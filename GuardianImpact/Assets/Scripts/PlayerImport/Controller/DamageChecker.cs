using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageChecker : MonoBehaviourPunCallbacks
{
    CharacterInformation characterInformation;

    // test variables
    private int attackerIndex = -1;
    private int attackerSentIndex = -2;

    private void Awake()
    {
        characterInformation = GetComponent<CharacterInformation>();
    }

    private void Start()
    {
        if (attackerIndex != -1) attackerIndex = -1;
        if (attackerSentIndex != -2) attackerSentIndex = -2;

        Debug.Log("Woah");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sphere" && other.CompareTag("Player"))
        {
            // saves the attackers actor number to attacker variable
            attackerIndex = other.gameObject.GetPhotonView().Owner.ActorNumber;

            Debug.Log("Collision Detected");
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Sphere" && collision.gameObject.CompareTag("Player"))
    //    {
    //        // saves the attackers actor number to attacker variable
    //        attacker = collision.gameObject.GetPhotonView().OwnerActorNr;

    //        Debug.Log("Collision Detected");
    //    }
    //}

    // method called by attacker
    [PunRPC]
    public void GotHit(int attackerSent)
    {
        attackerSentIndex = attackerSent;

        // start coroutine to validate attack sending the attackers number from detected collision
        StartCoroutine("ValidateAttack");

        Debug.Log("Got hit by number " + attackerIndex);
    }

    IEnumerator ValidateAttack()
    {
        // delay for case of lag compensation
        yield return new WaitForSecondsRealtime(0.1f);

        // checks to see if the number of the attacker is the same number that the attacker sent
        if (attackerIndex == attackerSentIndex)
        {
            if (photonView.IsMine)
            {
                // tells server to tell everyone to run the damage calculation method
                photonView.RPC("DamageCalculation", RpcTarget.AllViaServer);

                Debug.Log("Sent to calculate damage");
            }

            Debug.Log("Validating Attack");
        }

        // reset number to never used actor number
        attackerIndex = -1;
        attackerSentIndex = -2;
    }

    [PunRPC]
    void DamageCalculation()
    {
        characterInformation.health -= 10f;
        // health goes down

        Debug.Log("Damage calculated");
    }

    //[PunRPC]
    //void SimpleDamage()
    //{
    //    if (!photonView.IsMine)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        characterInformation.health = -10f;
    //    }
    //}
}
