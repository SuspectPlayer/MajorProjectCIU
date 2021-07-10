using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsMineComponentActivate : MonoBehaviourPunCallbacks
{
    public ThirdPersonMovement thirdPersonMovement;
    public GameObject playerCamera;

    public PhotonView photonView;

    private void Awake()
    {
        if (photonView == null) photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            thirdPersonMovement.enabled = true;
            playerCamera.SetActive(true);
        }
    }
}
