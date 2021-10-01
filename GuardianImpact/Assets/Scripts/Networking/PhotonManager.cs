using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager master;
    bool loggedIntoPlayfab;
    string gameVersion = "0.001";
    [SerializeField] bool forceJoinWaitingServer;
    [SerializeField] Button matchmakingButton;
    [SerializeField] Button cancelMatchmakingButton;
    [SerializeField] GameObject messageBox;
    [SerializeField] TextMeshProUGUI playersInRoomText;
    [SerializeField] byte maxPlayers = 8;
    [SerializeField] int waitingServerIndex = 4;

    GameObject acrossScenesObject;


    #region Monobehavior methods
    // Start is called before the first frame update
    void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
        // If you are in a room and the owner loads a new scene it will load the same scene for everyone in the room
        PhotonNetwork.AutomaticallySyncScene = forceJoinWaitingServer ? true : false ;
        IdleMenu();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (PhotonNetwork.IsConnectedAndReady) PhotonNetwork.Disconnect();
        }
    }

    #endregion Monobehavior methods

    #region Photon custom functions
    void JoinRandomRoom(byte expectedMaxPlayers)
    {
        PhotonNetwork.JoinRandomRoom(null, expectedMaxPlayers, MatchmakingMode.FillRoom, TypedLobby.Default, null);
    }
    void CreateRoom(byte maxPlayers)
    {
        // Create a new RoomOptions
        RoomOptions thisRoomOptions = new RoomOptions() { MaxPlayers = maxPlayers, PublishUserId = false };
        // Create a hash table for custom options
        ExitGames.Client.Photon.Hashtable customOptions = new ExitGames.Client.Photon.Hashtable();
        // Create a room
        PhotonNetwork.CreateRoom(null, thisRoomOptions);
    }
    void UpdatePlayersInRoom() 
    {
        playersInRoomText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
    #endregion Photon custom functions

    #region Photon override functions
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayersInRoom();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayersInRoom();
    }
    public override void OnJoinedRoom()
    {

        if (forceJoinWaitingServer) PhotonNetwork.LoadLevel(waitingServerIndex);
        else
        {
            // Load in the lobby level
            PanelMessagesManager.master.InstantiateMessage($"Room joined!", PanelMessageColor.neutralColor);
            matchmakingButton.gameObject.SetActive(false);
            cancelMatchmakingButton.gameObject.SetActive(true);
            UpdatePlayersInRoom();
            acrossScenesObject = PhotonNetwork.Instantiate("AcrossScenesObject", Vector3.zero, Quaternion.identity, 0);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        PanelMessagesManager.master.InstantiateMessage($"{message}", PanelMessageColor.headerFailTextColor);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log($"Player failed to join a room because : {message}.");
        CreateRoom(maxPlayers);
    }
    /// <summary>
    /// When player leaves the rooom
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        matchmakingButton.gameObject.SetActive(true); matchmakingButton.interactable = true;
        cancelMatchmakingButton.gameObject.SetActive(false);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        PanelMessagesManager.master.InstantiateMessage($"Disconnected from server!", PanelMessageColor.headerFailTextColor);
        PanelMessagesManager.master.InstantiateMessage($"*{cause}", PanelMessageColor.regularFailTextColor);
        matchmakingButton.gameObject.SetActive(false);
        cancelMatchmakingButton.gameObject.SetActive(false);
        PlayfabManager.master.ForceLogOutPlayfab();

        SaveUsername(string.Empty);

    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PanelMessagesManager.master.InstantiateMessage($"Connected to the server!", PanelMessageColor.regularSuccessTextColor);
        matchmakingButton.gameObject.SetActive(true); matchmakingButton.interactable = true;
        cancelMatchmakingButton.gameObject.SetActive(false);

        if (CharacterSelector.master != null) CharacterSelector.master.SetMaleCharacter();

        PlayfabManager.master.ReturnUsername();
    }
    #endregion Photon override functions

    #region Other functions
    public bool IsConnectedToServer()
    {
        return PhotonNetwork.IsConnected;
    }
    /// <summary>
    /// Called from Playfab Manager
    /// </summary>
    /// <param name="loggedIn">Logged in to Playfab status</param>
    public void SetLoggedInToPlayfabStatus(bool loggedIn)
    {
        Debug.Log($"Logged in is : {loggedIn}.");
        loggedIntoPlayfab = loggedIn;
        if(loggedIn)
        {
            ConnectToPhoton();
        }
        if(!loggedIn)
        {
            DisconnectFromPhoton();
        }
    }
    void ConnectToPhoton()
    {
        // Connect to the server
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();

            matchmakingButton.gameObject.SetActive(false);
            cancelMatchmakingButton.gameObject.SetActive(false);
        }
    }
    void DisconnectFromPhoton()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        matchmakingButton.gameObject.SetActive(false);
        cancelMatchmakingButton.gameObject.SetActive(false);
    }
    public void SaveUsername(string name)
    {
        ExitGames.Client.Photon.Hashtable newHash = new ExitGames.Client.Photon.Hashtable();
        newHash.Add("u", name);
        PhotonNetwork.LocalPlayer.SetCustomProperties(newHash);
        Debug.Log($"Username : '{name}' is saved.");
    }
    #endregion Other functions
    #region Button functions
    public void LookForPlayers()
    {
        matchmakingButton.gameObject.SetActive(false);
        if (!forceJoinWaitingServer)
        {
            messageBox.SetActive(true);
            cancelMatchmakingButton.gameObject.SetActive(true);
            JoinGameTimer.master.timerOn = true;
        }
        // Join a room
        JoinRandomRoom(maxPlayers);
    }
    public void IdleMenu()
    {
        messageBox.SetActive(false);
        matchmakingButton.gameObject.SetActive(true);
        cancelMatchmakingButton.gameObject.SetActive(false);
        JoinGameTimer.master.timerOn = false;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }
    public void JoinWaitingServer()
    {
        if (!PhotonNetwork.InRoom) return;
        if (!forceJoinWaitingServer)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
        }
        PhotonNetwork.LoadLevel(waitingServerIndex);
    }
    #endregion Button functions
}
