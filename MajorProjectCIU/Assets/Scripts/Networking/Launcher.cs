using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

// master script for all the programming for networking
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] string gameVersion = "0.1";
    [SerializeField] TMP_InputField roomNameInputField;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // if build version isn't the same, networking won't work
        PhotonNetwork.GameVersion = gameVersion.ToString();
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Connecting online");
    }

    #region Connection to online networking

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("Connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("Disconnected to server for {0}", cause.ToString());
    }

    #endregion

    #region Join main lobby

    // should be added to the buttons OnClick() events when the button for going to the main lobby is pressed
    public void JoinMainLobby()
    {
        PhotonNetwork.JoinLobby();
        MenuManager.Instance.OpenMenu("MainLobby");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined main lobby");
    }

    #endregion

    #region Leave main lobby

    public void LeaveMainLobby()
    {
        PhotonNetwork.LeaveLobby();
        MenuManager.Instance.OpenMenu("Main");

        Debug.Log("Left main lobby");
    }

    #endregion

    #region Create room

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);

        Debug.Log("Created room");
    }

    #endregion

    #region Join room

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);

        Debug.Log("Joining room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("Failed to connect to room {0}", message);
    }

    #endregion

    #region Leave room

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        Debug.Log("Left room");
    }

    #endregion
}
