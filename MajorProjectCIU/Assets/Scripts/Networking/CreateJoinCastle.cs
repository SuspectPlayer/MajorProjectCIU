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

    public void JoinCastleRoom()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable { { MAP, 1 } };

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayersPerCastle);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateCastleRoom();
    }

    void CreateCastleRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { MAP };
        roomOptions.MaxPlayers = maxPlayersPerCastle;
        roomOptions.BroadcastPropsChangeToAll = true;

        roomOptions.CustomRoomProperties = new Hashtable() { { MAP, 1 } };
        //roomOptions.IsVisible = false;

        PhotonNetwork.CreateRoom(null, roomOptions);
        PhotonNetwork.LoadLevel(1);
    }
}
