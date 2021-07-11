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

    public void CreateOrJoinCastle()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable() 
        {
            { MAP, 0 }
        };
        MatchmakingMode matchmakingMode = MatchmakingMode.RandomMatching;
        string roomName = $"Room {Random.Range(0, 20)}";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;

        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties, maxPlayersPerCastle, matchmakingMode,
            null, null, roomName, roomOptions);
        PhotonNetwork.LoadLevel(1);
    }
}
