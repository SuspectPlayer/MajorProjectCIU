using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using Photon.Realtime;

public class DamageManager : MonoBehaviourPunCallbacks
{

    void HitPlayer(GameObject defendingPlayer)
    {
        int attacker = PhotonNetwork.LocalPlayer.ActorNumber;
        int defender = defendingPlayer.GetPhotonView().Owner.ActorNumber;

        ValidateAttack(attacker, defender);
    }

    void GotHit(GameObject attackingPlayer)
    {
        // references the parameter of the attacker
        int attacker = attackingPlayer.GetPhotonView().Owner.ActorNumber;
        // references the defender or local player
        int defender = PhotonNetwork.LocalPlayer.ActorNumber;

        // send the attacker and defender information to get checked if true
        ValidateAttack(attacker, defender);
    }

    void ValidateAttack(int attacker, int defender)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/* if defending */other.CompareTag("Player"))
        {
            // calls GotHit() and send information of the attacker
            GotHit(other.gameObject);
        }
        else if (/* if attacking*/ other.CompareTag("Player"))
        {
            HitPlayer(other.gameObject);
        }
    }

    IEnumerator CalculateAttack()
    {


        yield return new WaitForSecondsRealtime(0.25f);

        // reset this coroutine
    }
}
