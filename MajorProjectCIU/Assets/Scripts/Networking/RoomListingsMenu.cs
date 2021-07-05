using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomListing roomListingPrefab;
    [SerializeField]
    private Transform roomListingContent;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfrom in roomListingContent)
        {
            Destroy(transfrom.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            Instantiate(roomListingPrefab, roomListingContent).GetComponent<RoomListing>().SetRoomInfo(roomList[i]);
        }
        // join main lobby to fix room dissapearing bug
        Debug.Log("List updated");
    }
}
