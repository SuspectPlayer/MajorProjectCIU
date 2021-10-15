using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamagePlayer : MonoBehaviourPun
{
    List<int> targetsHitThisTime = new List<int>();
    PlayerSync playerSync;
    private void Awake()
    {
        playerSync = GetComponentInParent<PlayerSync>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            // Get the photonview of the target
            PhotonView targetView = other.GetComponent<DamageChecker>().photonView;

            // This player is the attacker, send RPC over the server
            if (photonView.IsMine)
            {
                // If we already dealth damage to this player, return
                if (targetsHitThisTime.Contains(targetView.OwnerActorNr)) return;

                // Call the GotHit mehtod on the target
                targetView.RPC("GotHitRemote", targetView.Owner, photonView.Owner, playerSync.CurrentAnimatorSequence);
                targetsHitThisTime.Add(targetView.OwnerActorNr);
                Debug.Log($"{photonView.OwnerActorNr} hit player {targetView.OwnerActorNr} (Remote)");
            }
            // This is someone elses player doing the attack. Check if we're being attacked and do the sync check if that's true
            else
            {
                //Debug.Log($"Target is {targetView.OwnerActorNr}. Trigger detected on {GetComponentInParent<PhotonView>().OwnerActorNr}.");
                // If we already dealth damage to this player, return
                if (targetsHitThisTime.Contains(targetView.OwnerActorNr)) return;
                Photon.Realtime.Player attackerPlayer = GetComponentInParent<PhotonView>().Owner;
                other.GetComponent<DamageChecker>().GotHitLocal(attackerPlayer);
            }
        }
    }

    private void OnDisable()
    {
        // Reset the list of hit targets when this gameobject is disabled
        targetsHitThisTime.Clear();
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
