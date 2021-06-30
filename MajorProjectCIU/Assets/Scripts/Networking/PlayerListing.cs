using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text playerName;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;

        playerName.text = Player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (Player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
