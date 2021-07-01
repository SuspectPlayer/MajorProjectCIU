using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    [Header("Build Settings")]
    [SerializeField] string buildVersion = "0.1";
    [SerializeField] TMP_Text buildText;

    [Header("De-activated Buttons")]
    [SerializeField] List<Button> mainMenuButtons;

    [Header("Room Lobby")]
    [SerializeField] TMP_InputField roomNameInputField;
    [HideInInspector] public bool fromRoomLobby = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // stops from pressing multiplayer button until connected to main lobby
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = false;
        }
        // if build version isn't the same, networking won't work
        PhotonNetwork.GameVersion = buildVersion.ToString();
        PhotonNetwork.ConnectUsingSettings();
        buildText.text = "Build Version " + buildVersion;

        Debug.Log("Connecting online");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!fromRoomLobby)
        {
            foreach (Button button in mainMenuButtons)
            {
                button.interactable = true;
            }

            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000"); // sets the player name to a random name for now till player profiles are made
            Debug.Log(PhotonNetwork.NickName + " has joined the server");
        }
        else
        {
            PhotonNetwork.JoinLobby();
            fromRoomLobby = false;
            Debug.Log("Connected to master from room lobby");
        }

        Debug.Log("Connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("Disconnected to server for {0}", cause.ToString());
    }

    public void JoinMainLobby()
    {
        PhotonNetwork.JoinLobby();
        MenuManager.Instance.OpenMenu("MainLobby");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined main lobby");
    }

    public void LeaveMainLobby()
    {
        PhotonNetwork.LeaveLobby();
        MenuManager.Instance.OpenMenu("Main");

        Debug.Log("Left main lobby");
    }

    public void CreateNewRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)
            || !PhotonNetwork.IsConnected)
        {
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.EmptyRoomTtl = 0;

        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        MenuManager.Instance.OpenMenu("RoomLobby");

        Debug.Log("Created room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " " + message);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.Instance.OpenMenu("RoomLobby");

        Debug.Log("Joining room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("Failed to connect to room {0}", message);
    }

    public void LeaveRoomLobby()
    {
        PhotonNetwork.LeaveRoom();
        fromRoomLobby = true;
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("MainLobby");

        Debug.Log("Left room");
    }
}
