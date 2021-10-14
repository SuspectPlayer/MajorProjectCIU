using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class DamageChecker : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    protected class DamageSyncChecker
    {
        public int id;
        public bool remoteComfirmed;
        public bool localComfirmed;
        public float time;
    }

    CharacterInformation characterInformation;
    [SerializeField] float syncTime = 0.15f;
    // test variables
    private int attackerIndex = -1;
    private int attackerSentIndex = -2;


    private List<DamageSyncChecker> damageSyncCheckers; 

    private void Awake()
    {
        characterInformation = GetComponent<CharacterInformation>();
        damageSyncCheckers = new List<DamageSyncChecker>();
    }

    private void Start()
    {
        if (attackerIndex != -1) attackerIndex = -1;
        if (attackerSentIndex != -2) attackerSentIndex = -2;

        Debug.Log("Woah");
    }

    private void Update()
    {
        LoopThroughSyncCheckers();
    }
    void LoopThroughSyncCheckers()
    {
        List<DamageSyncChecker> surviveToNextFrame = new List<DamageSyncChecker>();

        for (int i = 0; i < damageSyncCheckers.Count; i++)
        {
            bool survive = true;
            // Time has run out
            if (damageSyncCheckers[i].time > syncTime)
            {
                survive = false;
                Validation(damageSyncCheckers[i].id, false);
            }
            // Validated successfully
            if (damageSyncCheckers[i].localComfirmed && damageSyncCheckers[i].remoteComfirmed)
            {
                survive = false;
                Validation(damageSyncCheckers[i].id, true);
            }
            damageSyncCheckers[i].time += Time.deltaTime;

            if (survive) surviveToNextFrame.Add(damageSyncCheckers[i]);
        }
        damageSyncCheckers = surviveToNextFrame;
    }
    void Validation(int attacker, bool succeeded)
    {
        if(succeeded)
        {
            Debug.Log($"Validation succeeded. {attacker} comfirmed as attacker on {photonView.OwnerActorNr}.");
        }
        else
        {
            Debug.Log($"Validation failed. {attacker} failed to attack {photonView.OwnerActorNr}. Time ran out.");
        }
    }

    /* private void OnTriggerEnter(Collider other)
     {
         if (other.name == "Sphere" && other.CompareTag("Player"))
         {
             // saves the attackers actor number to attacker variable
             attackerIndex = other.gameObject.GetPhotonView().Owner.ActorNumber;

             Debug.Log("Collision Detected");
         }
     }*/

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
    public void GotHitRemote(int attackerSent)
    {
        attackerSentIndex = attackerSent;

        // start coroutine to validate attack sending the attackers number from detected collision
        //StartCoroutine("ValidateAttack");

        Debug.Log($"{photonView.OwnerActorNr} hit by {attackerSent} (Remote)");
        AddIDToDamageSync(attackerSent, false);
    }
    public void GotHitLocal(int attackerSent)
    {
        Debug.Log($"{photonView.OwnerActorNr} hit by {attackerSent} (Local)");
        AddIDToDamageSync(attackerSent, true);
    }

    void AddIDToDamageSync(int attackerID, bool local)
    {
        // Try to find an item in the list that matches the attacker ID, returns -1 if one can't be found
        int index = damageSyncCheckers.FindIndex(item => item.id == attackerID);

        if (index >= 0)
        {
            // element exists, do what you need
            if (local) damageSyncCheckers[index].localComfirmed = true;
            else damageSyncCheckers[index].remoteComfirmed = true;
        }
        else
        {
            // Create a new item and add the variables
            DamageSyncChecker newItem = new DamageSyncChecker();
            newItem.id = attackerID;
            if (local) newItem.localComfirmed = true;
            else newItem.remoteComfirmed = true;

            damageSyncCheckers.Add(newItem);
        }

    }

    /*IEnumerator ValidateAttack()
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
    }*/

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
