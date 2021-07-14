using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsMineComponentActivate : MonoBehaviourPunCallbacks
{
    public StarterAssets.ThirdPersonController thirdPersonController;
    public GameObject mainCamera;
    public GameObject playerCamera;

    public PhotonView photonView;

    private void Awake()
    {
        if (photonView == null) photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            thirdPersonController.enabled = true;
            playerCamera.SetActive(true);
            mainCamera.SetActive(true);
        }
    }
}
