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
        public Photon.Realtime.Player player;
        public AnimatorSequence attackerSequence;
        public bool remoteComfirmed;
        public bool localComfirmed;
        public float time;
    }

    CharacterInformation characterInformation;
    PlayerSync defenderSync;
    BasicBehaviour basicBehavior;
    [SerializeField] float syncTime = 0.15f;
    // test variables
    private int attackerIndex = -1;
    private int attackerSentIndex = -2;


    private List<DamageSyncChecker> damageSyncCheckers; 

    private void Awake()
    {
        characterInformation = GetComponent<CharacterInformation>();
        defenderSync = GetComponent<PlayerSync>();
        basicBehavior = GetComponent<BasicBehaviour>();
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
                Validation(damageSyncCheckers[i], false);
            }
            // Validated successfully
            if (damageSyncCheckers[i].localComfirmed && damageSyncCheckers[i].remoteComfirmed)
            {
                survive = false;
                Validation(damageSyncCheckers[i], true);
            }
            damageSyncCheckers[i].time += Time.deltaTime;

            if (survive) surviveToNextFrame.Add(damageSyncCheckers[i]);
        }
        damageSyncCheckers = surviveToNextFrame;
    }
    void Validation(DamageSyncChecker attacker, bool succeeded)
    {
        if(succeeded)
        {
            Debug.Log($"Validation succeeded. {attacker.id} comfirmed as attacker on {photonView.OwnerActorNr}.");
            ReactionToAttack(attacker);

        }
        else
        {
            Debug.Log($"Validation failed. {attacker.id} failed to attack {photonView.OwnerActorNr}. Time ran out.");
        }
    }
    // This is running on the targets/defenders machine
    void ReactionToAttack(DamageSyncChecker attacker)
    {
        AnimatorSequence defenderSequence = defenderSync.CurrentAnimatorSequence;
        Debug.Log($"ReactionToAttack is running. Attacker's AnimatorSequence is {attacker.attackerSequence}. Defender's AnimationSequence is {defenderSequence}.");
        string stateNameLocal = string.Empty;
        string stateNameRemote = string.Empty;
        if(defenderSequence == AnimatorSequence.counter)
        {
            stateNameLocal = "CounterAttack";
        }
        // Defender is dodging
        else if(defenderSequence == AnimatorSequence.dodge)
        {
            stateNameLocal = string.Empty;
        }
        // Attacker does a regular attack
        else if(attacker.attackerSequence == AnimatorSequence.first || attacker.attackerSequence == AnimatorSequence.second)
        {
            // If attacker and defender are both in either state 1 or 2, swords will clash
            if (defenderSequence == AnimatorSequence.first || defenderSequence == AnimatorSequence.second) stateNameLocal = stateNameRemote = "SwordClash";
            else if(defenderSequence == AnimatorSequence.third)
            {
                stateNameRemote = "GetHitRegular";
            }
            else if(defenderSequence == AnimatorSequence.airAttack)
            {
                stateNameRemote = "GetHitAir";
            }
            else
            {
                stateNameLocal = "GetHitRegular";
            }
        }
        // Attacker does a power attack
        else if(attacker.attackerSequence == AnimatorSequence.third || attacker.attackerSequence == AnimatorSequence.airAttack)
        {
            if (defenderSequence == AnimatorSequence.dodge) stateNameLocal = string.Empty;
            else if (defenderSequence == AnimatorSequence.third || defenderSequence == AnimatorSequence.airAttack) stateNameLocal = stateNameRemote = "SwordClash";
        }

        if (stateNameLocal != string.Empty) {
            stateNameLocal = "Base Layer." + stateNameLocal;
            Debug.Log($"Trying to play state : {stateNameLocal} on {photonView.OwnerActorNr}");
            basicBehavior.GetAnim.Play(stateNameLocal);
        }
        if(stateNameRemote != string.Empty)
        {
            stateNameRemote = "Base Layer." + stateNameRemote;
            Debug.Log($"Trying to play state : {stateNameRemote} on {attacker.id}");
            photonView.RPC("ReturnCallFromTarget", attacker.player, stateNameRemote);
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

    [PunRPC]
    public void ReturnCallFromTarget(string returnAnimation)
    {
        basicBehavior.GetAnim.Play(returnAnimation);
    }

    // method called by attacker
    [PunRPC]
    public void GotHitRemote(Photon.Realtime.Player attackerSent, AnimatorSequence attackerSequence)
    {
        attackerSentIndex = attackerSent.ActorNumber;

        // start coroutine to validate attack sending the attackers number from detected collision
        //StartCoroutine("ValidateAttack");

        Debug.Log($"{photonView.OwnerActorNr} hit by {attackerSent.ActorNumber} (Remote)");
        AddIDToDamageSync(attackerSent, attackerSequence, false);
    }
    public void GotHitLocal(Photon.Realtime.Player attackerSent)
    {
        Debug.Log($"{photonView.OwnerActorNr} hit by {attackerSent} (Local)");
        AddIDToDamageSync(attackerSent, AnimatorSequence.none, true);
    }

    void AddIDToDamageSync(Photon.Realtime.Player attackerSent, AnimatorSequence attackerSequence, bool local)
    {
        // Try to find an item in the list that matches the attacker ID, returns -1 if one can't be found
        int index = damageSyncCheckers.FindIndex(item => item.id == attackerSent.ActorNumber);

        if (index >= 0)
        {
            // element exists, do what you need
            if (local) damageSyncCheckers[index].localComfirmed = true;
            else
            {
                damageSyncCheckers[index].attackerSequence = attackerSequence;
                damageSyncCheckers[index].remoteComfirmed = true;
            }
        }
        else
        {
            // Create a new item and add the variables
            DamageSyncChecker newItem = new DamageSyncChecker();
            newItem.id = attackerSent.ActorNumber;
            newItem.player = attackerSent;
            newItem.attackerSequence = attackerSequence;
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
