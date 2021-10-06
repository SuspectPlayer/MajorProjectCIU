using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager master;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] Toggle rememberUser;

    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;

    string thisUserID = string.Empty;
    string thisUsername = string.Empty;

    bool isLoggedIn;

    #region Monobehavior methods
    private void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
    }
    private void Start()
    {
        LoggedInSettings(false);
        rememberUser.onValueChanged = new Toggle.ToggleEvent();
        if (PlayerPrefs.HasKey("username"))
        {
            rememberUser.isOn = true;
            username.text = PlayerPrefs.GetString("username");
            password.text = PlayerPrefs.GetString("password");
            LoginButton();
        }
        rememberUser.onValueChanged.AddListener(SaveUserToggle);
    }
    private void Update()
    {
        // Shift between the username- and password input fields
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            // If the user is already logged in, return
            if (IsLoggedIn()) return;
            // If the username input field is selected, switch to the password input field
            if (username.isFocused) password.ActivateInputField();
            // If the password input field is selected, switch to the username input field
            else if (password.isFocused) username.ActivateInputField();
            // Nothing is selected, select the username input field
            else username.ActivateInputField();
        }
        // Try loggin in if the user presses Enter/Return
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // If the user is already logged in, return
            if (IsLoggedIn()) return;
            // Log in
            LoginButton();
        }
    }
    private void LateUpdate()
    {
        // Check if the player has been logged out. A safety meassure that may be disabled without loosing functionality
        if (isLoggedIn && !IsLoggedIn()) LoggedInSettings(false);
    }
    #endregion Monobehavior methods
    #region Other functions
    /// <summary>
    /// Check if the user is logged in on Playfab
    /// </summary>
    /// <returns>logged in bool</returns>
    public bool IsLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }
    /// <summary>
    /// Activate the UI input fields and buttons based on if the user is logged in to Playfab or not
    /// </summary>
    /// <param name="loggedIn"></param>
    void LoggedInSettings(bool loggedIn)
    {
        isLoggedIn = loggedIn;

        if (loggedIn && rememberUser.isOn) SaveUser();

        username.interactable = !loggedIn;
        password.interactable = !loggedIn;

        loginButton.interactable = !loggedIn;
        loginButton.gameObject.SetActive(!loggedIn);
        registerButton.interactable = !loggedIn;
        registerButton.gameObject.SetActive(!loggedIn);

        PhotonManager.master.SetLoggedInToPlayfabStatus(loggedIn);
    }
    /// <summary>
    /// Force the player to log out from Playfab (e.g. if creating a Photon server fails).
    /// This is a fallback funcitonality to force the user to start from the beginning. It shouldn't be called unless somehting goes very wrong.
    /// </summary>
    public void ForceLogOutPlayfab()
    {
        if(IsLoggedIn())
        {
            PlayFabClientAPI.ForgetAllCredentials();
            LoggedInSettings(IsLoggedIn());
            PanelMessagesManager.master.InstantiateMessage("Logged out!", PanelMessageColor.neutralColor);
            thisUserID = string.Empty;
        }
    }
    public void ReturnUsername()
    {
       GetUsername(thisUserID);
    }
    void GetUsername(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => {
            PhotonManager.master.SaveUsername(result.PlayerProfile.DisplayName);
        },
        error => {
            PhotonManager.master.SaveUsername(string.Empty);
        });
    }

    
    #endregion Other funcitons
    #region Button funcitons
    public void SaveUserToggle(bool on)
    {
        if (on && PlayFabClientAPI.IsClientLoggedIn()) SaveUser();
        else DeleteUser();
    }
    void SaveUser()
    {
        PlayerPrefs.SetString("username", username.text);
        PlayerPrefs.SetString("password", password.text);
    }
    void DeleteUser()
    {
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("password");
    }
    public void LoginButton()
    {
        if (IsLoggedIn()) return;
        var request = new LoginWithPlayFabRequest
        {
            Username = username.text,
            Password = password.text,
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    }
    public void RegisterButton()
    {
        if (IsLoggedIn()) return;

        var request = new RegisterPlayFabUserRequest
        {
            Username = username.text,
            DisplayName = username.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false,

        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    #endregion Button functions
    #region Playfab callbacks
    /// <summary>
    /// Called when a Playfab login-user request is successful
    /// </summary>
    /// <param name="obj">The LoginResult generated by Playfab</param>
    private void OnLoginSuccess(LoginResult obj)
    {
        PanelMessagesManager.master.InstantiateMessage($"Logged in!", PanelMessageColor.regularSuccessTextColor);
        LoggedInSettings(true);
        thisUserID = obj.PlayFabId;
    }
    /// <summary>
    /// Called when a Playfab register-user request is successful
    /// </summary>
    /// <param name="obj">The RegisterPlayfabUserResult generated by Playfab</param>
    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        PanelMessagesManager.master.InstantiateMessage($"Created an account for : {obj.Username}!", PanelMessageColor.regularSuccessTextColor);
        //username.text = string.Empty;
        //password.text = string.Empty;
        //username.ActivateInputField();

        LoggedInSettings(true);
        thisUserID = obj.PlayFabId;
    }
    /// <summary>
    /// Called when a Playfab user request returns an error
    /// </summary>
    /// <param name="obj">Error that is generated by Playfab</param>
    private void OnError(PlayFabError obj)
    {
        PanelMessagesManager.master.InstantiateMessage(obj.ErrorMessage, PanelMessageColor.headerFailTextColor);

        if (obj.ErrorDetails == null) return;
        Dictionary<string, List<string>> errors = obj.ErrorDetails;
        foreach(KeyValuePair<string, List<string>> KVP in errors)
        {
            for (int i = 0; i < KVP.Value.Count; i++)
            {
                if (KVP.Value[i].Contains("One of the following")) continue;
                Debug.Log(KVP.Value[i]);

                PanelMessagesManager.master.InstantiateMessage($"*{KVP.Value[i]}", PanelMessageColor.regularFailTextColor);
            }
        }
        thisUsername = string.Empty;
    }
    #endregion Playfab callbacks
}
