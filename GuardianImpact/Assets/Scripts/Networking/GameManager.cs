using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string[] playerPrefabs;
    [SerializeField] Transform[] spawnPoints;
    int playerPrefabIndex;
    [SerializeField] string path;


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        ExitGames.Client.Photon.Hashtable thisPlayerHash = PhotonNetwork.LocalPlayer.CustomProperties;
        if (thisPlayerHash.ContainsKey("i"))
        {
            playerPrefabIndex = (int)thisPlayerHash["i"];
        }
        if(thisPlayerHash.ContainsKey("u"))
        {
            Debug.Log($"The players stored username is {(string)thisPlayerHash["u"]}");
        }
        else playerPrefabIndex = 0;
        Debug.Log($"Player index is {playerPrefabIndex}.");
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
    }
}
