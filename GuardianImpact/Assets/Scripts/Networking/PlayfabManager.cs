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
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;

    [SerializeField] GameObject panelMessagePrefab;
    [SerializeField] Transform messagePanelContent;

    [SerializeField] Color headerFailTextColor = Color.white;
    [SerializeField] Color regularFailTextColor = Color.white;
    [SerializeField] Color regularSuccessTextColor = Color.white;

    public bool IsLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
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
            Password = password.text,
            RequireBothUsernameAndEmail = false,

        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        InstantiateMessage($"Logged in!", regularSuccessTextColor);
        username.interactable = false;
        password.interactable = false;
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        InstantiateMessage($"Created an account for : {obj.Username}!", regularSuccessTextColor);
        username.text = string.Empty;
        password.text = string.Empty;
    }

    void InstantiateMessage(string message, Color messageColor)
    {
        GameObject newMessage = Instantiate(panelMessagePrefab, messagePanelContent);
        newMessage.transform.GetComponent<PanelMessage>().SetText(message);
        newMessage.transform.GetComponent<TextMeshProUGUI>().color = messageColor;
    }

    private void OnError(PlayFabError obj)
    {
        InstantiateMessage(obj.ErrorMessage, headerFailTextColor);

        Dictionary<string, List<string>> errors = obj.ErrorDetails;
        foreach(KeyValuePair<string, List<string>> KVP in errors)
        {
            for (int i = 0; i < KVP.Value.Count; i++)
            {
                if (KVP.Value[i].Contains("One of the following")) continue;
                Debug.Log(KVP.Value[i]);

                InstantiateMessage($"*{KVP.Value[i]}", regularFailTextColor);

            }
        }
    }
}
