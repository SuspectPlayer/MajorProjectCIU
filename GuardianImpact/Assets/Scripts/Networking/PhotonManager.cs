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

    [SerializeField] Button matchmakingButton;
    [SerializeField] Button cancelMatchmakingButton;
    [SerializeField] byte maxPlayers = 8;


    #region Monobehavior methods
    // Start is called before the first frame update
    void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
        // If you are in a room and the owner loads a new scene it will load the same scene for everyone in the room
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (PhotonNetwork.IsConnectedAndReady) PhotonNetwork.Disconnect();
        }
    }

    #endregion Monobehavior methods

    #region Photon functions
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
    public override void OnJoinedRoom()
    {
        // Load in the lobby level
        Debug.Log($"Player successfully joined a room");
        matchmakingButton.gameObject.SetActive(false);
        cancelMatchmakingButton.gameObject.SetActive(true);
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

    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PanelMessagesManager.master.InstantiateMessage($"Connected to the server!", PanelMessageColor.regularSuccessTextColor);
        matchmakingButton.gameObject.SetActive(true); matchmakingButton.interactable = true;
        cancelMatchmakingButton.gameObject.SetActive(false);
    }
    #endregion Photon functions

    #region Other functions
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
    #endregion Other functions
    #region Button functions
    public void StartMatchmakingButton()
    {
        // Turn off interactibility on the matchmaking button to avoid spam before the player has joined a room
        matchmakingButton.interactable = false;
        // Join a room
        JoinRandomRoom(maxPlayers);
    }
    #endregion Button functions
}
