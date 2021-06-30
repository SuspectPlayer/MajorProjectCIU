using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

// connects to the online servers
public class OnlineConnection : MonoBehaviourPunCallbacks
{
    public static OnlineConnection Instance;

    [SerializeField] string buildVersion = "0.1";
    [SerializeField] TMP_Text buildText;
    [SerializeField] GameObject multiplayerButton;

    [HideInInspector] public bool fromRoomLobby = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // stops from pressing multiplayer button until connected to main lobby
        multiplayerButton.GetComponent<Button>().interactable = false;
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
            multiplayerButton.GetComponent<Button>().interactable = true;
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

    // should be added to the buttons OnClick() events when the button for going to the main lobby is pressed
    public void JoinMainLobby()
    {
        PhotonNetwork.JoinLobby();
        MenuManager.Instance.OpenMenu("MainLobby");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000"); // sets the player name to a random name for now till player profiles are made
        Debug.Log(PhotonNetwork.NickName + " has joined the server");

        Debug.Log("Joined main lobby");
    }

    public void LeaveMainLobby()
    {
        PhotonNetwork.LeaveLobby();
        MenuManager.Instance.OpenMenu("Main");

        Debug.Log("Left main lobby");
    }
}
