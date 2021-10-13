using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit player");

        if (other.CompareTag("Player"))
        {
            other.GetComponent<DamageChecker>().photonView.RPC("GotHit", RpcTarget.AllViaServer);

            Debug.Log("Hit player");
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Hit player");

    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.gameObject.GetComponent<DamageChecker>().photonView.RPC("GotHit", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.ActorNumber);

    //        Debug.Log("Hit player");
    //    }
    //}
}
