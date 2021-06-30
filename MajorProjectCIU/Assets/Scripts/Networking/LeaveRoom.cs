using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public void LeaveRoomLobby()
    {
        PhotonNetwork.LeaveRoom();
        OnlineConnection.Instance.fromRoomLobby = true;
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("MainLobby");

        Debug.Log("Left room");
    }
}
