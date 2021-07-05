using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerListing playerListingPrefab;
    [SerializeField]
    private Transform redListingContent;
    [SerializeField]
    private Transform blueListingContent;

    [SerializeField]
    private TMP_Text roomName;

    [SerializeField]
    private GameObject startButton;

    public override void OnJoinedRoom()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            ChooseSide(0, PhotonNetwork.LocalPlayer);
            Debug.Log("On joined room, master client is choosing a side");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["RedTeam"] == (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueTeam"])
            {
                ChooseSide(0, newPlayer);
                Debug.Log("Master client is choosing a side for a joining player to the red team");
            }
            else
            {
                ChooseSide(1, newPlayer);
                Debug.Log("Master client is choosing a side for a joining player to the blue team");
            }
        }
    }

    public void ChooseSide(int sideIndex, Player player)
    {
        Hashtable newPlayerProperties = new Hashtable()
        {
            { "PlayerTeam", sideIndex }
        };

        player.SetCustomProperties(newPlayerProperties);

        Debug.Log("custom property initialized for player");

        if (sideIndex == 0)
        {
            int redTeamCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedTeam"];
            int newRedTeamCount = redTeamCount + 1;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() 
            {
                { "RedTeam", newRedTeamCount }
            });
            Debug.Log("Player on red team");
        }
        else
        {
            int blueTeamCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueTeam"];
            int newBlueTeamCount = blueTeamCount + 1;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() 
            {
                { "BlueTeam", newBlueTeamCount }
            });
            Debug.Log("Player on blue team");
        }
        Debug.Log("Finished choosing side now populating lists");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("on player properties update called");

        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        PopulateLists();

        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["RedTeam"] == (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueTeam"])
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void PopulateLists()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log("players list updating");

        foreach (Transform child in redListingContent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed red listing content");

        foreach (Transform child in blueListingContent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed blue listing content");

        foreach (Player player in players)
        {
            if ((int)player.CustomProperties["PlayerTeam"] == 0)
            {
                Instantiate(playerListingPrefab, redListingContent).GetComponent<PlayerListing>().SetPlayerInfo(player);
                Debug.Log("Instantiating red listing content");
            }
            else
            {
                Instantiate(playerListingPrefab, blueListingContent).GetComponent<PlayerListing>().SetPlayerInfo(player);
                Debug.Log("Instantiating blue listing content");
            }
        }
    }
}
