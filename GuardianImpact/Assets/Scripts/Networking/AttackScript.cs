using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReturnValueAttack
{
    hit,
    miss,
    swordClash,
    shieldClash,
    counter
}
public class AttackScript : MonoBehaviourPun
{
    PlayerSync playerSync;
    BasicBehaviour basicBehaviour;
    PlayerHealth healthScript;
    public Animator animator;
    int playerLayer = 1 << 6;
    [SerializeField] float overlapSphereSize = 4f;

    [SerializeField] float maxDamageRange = 2f;
    [Tooltip("The max angle to the target on the distance of 0")]
    [SerializeField] float interationAngleMinDistance = 90f;
    [Tooltip("The max angle to the target on the distance of maxDamageRange")]
    [SerializeField] float interationAngleMaxDistance = 20f;

    private void Start()
    {
        playerSync = GetComponent<PlayerSync>();
        basicBehaviour = GetComponent<BasicBehaviour>();
        healthScript = GetComponent<PlayerHealth>();
        animator = basicBehaviour.GetAnim;
    }
    public void Attack()
    {
        if (!photonView.IsMine) return;
        AnimatorSequence currentState = playerSync.CurrentAnimatorSequence;
        Collider[] overlapPlayers = Physics.OverlapSphere(transform.position, overlapSphereSize, playerLayer);
        List<int> targets = new List<int>();
        foreach (var collider in overlapPlayers)
        {
            PhotonView PV = collider.transform.GetComponent<PhotonView>();
            if (PV.ViewID == photonView.ViewID) continue;
            Debug.Log($"Attacker is {photonView.ViewID}. Defender is {PV.ViewID}");
            Photon.Realtime.Player sendToPlayer = PhotonNetwork.CurrentRoom.GetPlayer(PV.ViewID);
            photonView.RPC("RunOnTarget", sendToPlayer, currentState, photonView.ViewID, PV.ViewID);
        }
    }

    ReturnValueAttack CheckSequence(AnimatorSequence attackerSequence, AnimatorSequence defenderSequence)
    {
        Debug.Log($"AttackerSequence is {attackerSequence}. DefenderSequence is {defenderSequence}.");
        if(defenderSequence == AnimatorSequence.counterAttack)
        {
            // The target is countering
            return ReturnValueAttack.counter;
        } 
        else if(defenderSequence == AnimatorSequence.dodge)
        {
            // The target is dodgin
            return ReturnValueAttack.miss;
        }
        else if(defenderSequence == attackerSequence)
        {
            // The attackstates are the same for attacker and defender, swords will clash
            if (attackerSequence == AnimatorSequence.first || attackerSequence == AnimatorSequence.second || attackerSequence == AnimatorSequence.third || attackerSequence == AnimatorSequence.airAttack)
            {
                return ReturnValueAttack.swordClash;
            }
        }
        // Attacker is in the first state
        else if(attackerSequence == AnimatorSequence.first)
        {
            // Rock beats siccors
            if (defenderSequence == AnimatorSequence.second) return ReturnValueAttack.counter;
        }
        else if(attackerSequence == AnimatorSequence.second)
        {
            // Paper beats rocks
            if (defenderSequence == AnimatorSequence.third) return ReturnValueAttack.counter;
        }
        else if(attackerSequence == AnimatorSequence.third)
        {
            // Siccors beat paper
            if (defenderSequence == AnimatorSequence.first) return ReturnValueAttack.counter;
        }

        return ReturnValueAttack.hit;
    }

    string GetAnimatorValues(ReturnValueAttack setValue, AnimatorSequence animatorSequence, bool isAttacker)
    {
        //Debug.Log($"Setting Aniimator Values.");
        string state = string.Empty;

        // if (setValue == ReturnValueAttack.miss)

        if (setValue == ReturnValueAttack.hit)
        {
            //Debug.Log($"Set value is hit.");
            if (!isAttacker)
            {
                if (animatorSequence == AnimatorSequence.airAttack) state = "GetHitAirRegular";
                else state = "GetHitRegular";
            }
        }
        else if (setValue == ReturnValueAttack.counter)
        {
            if (!isAttacker) state = "CounterAttack";
        }
        else if(setValue == ReturnValueAttack.shieldClash)
        {
            if(isAttacker)
            {
                // Implement effects from hitting the shield?
            }
            else
            {
                // Our shield is being hit?
            }
        }
        else if(setValue == ReturnValueAttack.swordClash)
        {
            // Set the sword clash animation
            state = "SwordClash";
        }
        //Debug.Log($"State is set to : {state}.");
        return state;
    }

    [PunRPC] 
    void ReturnValue(ReturnValueAttack returnValueAttack, AnimatorSequence animatorSequence)
    {
        Debug.Log($"Running ReturnValue on {photonView.ViewID}");
        string state = GetAnimatorValues(returnValueAttack, animatorSequence, true);
        if (state != string.Empty) 
        {
            state = "Base Layer." + state;
            Debug.Log($"Trying to run {state} on {photonView.ViewID}");
            animator.Play(state);
        }
    }

    [PunRPC]
    void RunOnTarget(AnimatorSequence attackerSequence, int attackerID, int targetID)
    {
        Debug.Log($"RunOnTarget is running on target: {targetID} with attacker {attackerID}.");

        Transform attackerTransform = PhotonView.Find(attackerID).gameObject.transform;
        Transform targetTransform = PhotonView.Find(targetID).gameObject.transform;

        if (attackerTransform == null) return;
        // The player to return a value to
        Photon.Realtime.Player returnToPlayer = PhotonNetwork.CurrentRoom.GetPlayer(attackerID);

        float distance = Vector3.Distance(attackerTransform.position, targetTransform.position);
        Debug.Log($"Distance is {distance} out of {maxDamageRange}.");
        if (distance > maxDamageRange) return;

        float maxAngle = MaxAngleToInteract(distance);
        // Angle to target from attacker
        float angleToTarget = CheckAngle(targetTransform, attackerTransform);

        Debug.Log($"Angle to target from attacker is {angleToTarget} out of {maxAngle}.");
        if (angleToTarget > maxAngle) return;
        // At this stage we can attack

        // Angle to attacker from target
        float angleToAttacker = CheckAngle(attackerTransform, targetTransform);
        Debug.Log($"Angle to attacker from target is {angleToAttacker} out of {maxAngle}.");



        ReturnValueAttack returnValueAttack = ReturnValueAttack.hit;
        // Target can defend
        if (angleToAttacker <= maxAngle)
        {
            returnValueAttack = CheckSequence(attackerSequence, targetTransform.GetComponent<PlayerSync>().CurrentAnimatorSequence);
            Debug.Log($"Target can defend. ReturnValueAttack is {returnValueAttack}.");
        }

        string state = GetAnimatorValues(returnValueAttack, attackerSequence, false);
        if (state != string.Empty)
        {
            state = "Base Layer." + state; 
            Debug.Log($"Trying to play state : '{state}'. on {targetID}");
            // ERROR: It finds the right state but the animation never plays.
            animator.Play(state);
        }
        else Debug.Log($"state string is empty on {targetID}");
        photonView.RPC("ReturnValue", returnToPlayer, returnValueAttack, attackerSequence);

    }
    float MaxAngleToInteract(float distance)
    {
        if (distance > maxDamageRange) distance = maxDamageRange;
        float maxAngle = interationAngleMaxDistance - (interationAngleMaxDistance - interationAngleMinDistance) * (distance / maxDamageRange);
            return maxAngle;

    }
    float CheckAngle(Transform toPosition, Transform fromPosition)
    {
       
        var relativePos = toPosition.position - fromPosition.position;

        var forward = fromPosition.forward;
        var angle = Vector3.Angle(relativePos, forward);
        Vector3 crossProduct = Vector3.Cross(forward, relativePos);
        if (crossProduct.y < 0)
        {
            //Do left stuff
        }
        else
        {
            //Do right stuff
        }
        Debug.Log($"Checking angle from {fromPosition.GetComponent<PhotonView>().ViewID} to {toPosition.GetComponent<PhotonView>().ViewID} is {angle}.");
        return Mathf.Abs(angle);
    }
}
