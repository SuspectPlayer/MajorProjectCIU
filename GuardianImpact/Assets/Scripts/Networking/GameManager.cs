using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameManager : MonoBehaviourPun
{
    [SerializeField] string[] playerPrefabs;
    [SerializeField] Transform[] spawnPoints;
    int playerPrefabIndex;
    [SerializeField] string path;


    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        ExitGames.Client.Photon.Hashtable thisPlayerHash = PhotonNetwork.LocalPlayer.CustomProperties;
        if (thisPlayerHash.ContainsKey("i"))
        {
            playerPrefabIndex = (int)thisPlayerHash["i"];
        }
        else playerPrefabIndex = 0;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        string thePath = Path.Combine(path, playerPrefabs[playerPrefabIndex]);

        GameObject playerPrefab = PhotonNetwork.Instantiate(thePath, spawnPoint.position, spawnPoint.rotation, 0);
        // Send a RPC call to all other clients
        //photonView.RPC("SpawnOldPlayer", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer, playerPrefab);
    }
    [PunRPC]
    void SpawnOldPlayer(Photon.Realtime.Player player, GameObject playerPrefab)
    {
        GameObject[] playersInRoom = GameObject.FindGameObjectsWithTag("Player");
        bool existInRoom = false;
        foreach(GameObject GO in playersInRoom)
        {
            if(player.ActorNumber == GO.GetComponent<PhotonView>().OwnerActorNr)
            {
                existInRoom = true;
            }
        }
        if(!existInRoom) { }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}