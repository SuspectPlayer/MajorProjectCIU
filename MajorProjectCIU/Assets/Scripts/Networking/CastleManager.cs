using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CastleManager : MonoBehaviourPunCallbacks
{
    public string playerPrefabLocation;
    public Transform spawnpoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.Quit();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefabLocation, spawnpoint.position, Quaternion.identity);
    }
}
