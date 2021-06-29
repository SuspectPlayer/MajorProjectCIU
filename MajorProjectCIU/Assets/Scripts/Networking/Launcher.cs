using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// master script for all the programming for networking
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] string gameVersion = "0.1";

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

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("Connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("Disconnected to server for {0}", cause.ToString());
    }

    public void JoinMainLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined main lobby");
    }
}
