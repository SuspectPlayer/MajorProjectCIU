using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LeaveGame : MonoBehaviourPunCallbacks
{
    public void OnClick()
    {
        PhotonNetwork.LoadLevel(0);
        NetworkManager.Instance.fromRoomLobby = false;

        PhotonNetwork.LeaveRoom();
    }
}
