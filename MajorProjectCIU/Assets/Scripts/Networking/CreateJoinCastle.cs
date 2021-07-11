using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateJoinCastle : MonoBehaviourPunCallbacks
{
    public byte maxPlayersPerCastle = 10;

    public const string MAP = "map";

    public void CraeteOrJoinCastle()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable() 
        {
            { MAP, 0 }
        };
        MatchmakingMode matchmakingMode = MatchmakingMode.RandomMatching;

        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties, maxPlayersPerCastle, matchmakingMode);
        PhotonNetwork.LoadLevel(1);
    }
}
