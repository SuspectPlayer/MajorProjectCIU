using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CastleManager : MonoBehaviourPunCallbacks
{
    public string playerPrefabLocation;
    public Transform spawnpoint;

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefabLocation, spawnpoint.position, Quaternion.identity);
    }
}
