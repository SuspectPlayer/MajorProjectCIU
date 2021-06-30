using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerListing playerListingPrefab;
    [SerializeField]
    private Transform playerListingContent;

    [SerializeField]
    private GameObject roomName;

    [SerializeField]
    private GameObject startButton;

    private void Update()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        //{
        //    startButton.SetActive(PhotonNetwork.IsMasterClient);
        //}
    }

    public override void OnJoinedRoom()
    {
        if (gameObject.activeInHierarchy)
        {
            roomName.SetActive(true);
        }
        else
        {
            roomName.SetActive(false);
        }

        roomName.GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListingContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListingPrefab, playerListingContent).GetComponent<PlayerListing>().SetPlayerInfo(players[i]);
        }

        //startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListingPrefab, playerListingContent).GetComponent<PlayerListing>().SetPlayerInfo(newPlayer);
    }
}
