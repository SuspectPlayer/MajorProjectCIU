using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[System.Serializable]
public enum AnimatorSequence
{
    none,
    first,
    second,
    third,
    dodge,
    counter,
    counterAttack,
    swordClash,
    airAttack,
    sprint,
    charge,
    level2,
    level3,
    beingHit,



}
public class PlayerSync : MonoBehaviourPun, IPunObservable
{

    [SerializeField] bool syncRotation = false;
    [SerializeField] bool useSmoothing = true;
    [SerializeField] bool syncAnimator = true;
    [SerializeField] bool syncHealth = true;
    [SerializeField] float smoothingSpeed = 130f;
    Vector3 networkPosition;
    Quaternion networkRotation;

    [SerializeField] AnimatorSequence currentAnimatorSequence;
    public AnimatorSequence CurrentAnimatorSequence { get { return currentAnimatorSequence; } }

    Animator animator;
    PlayerHealth healthScript;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.gameObject;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthScript = GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        if (!useSmoothing || photonView.IsMine) return;
        if (syncRotation) transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, smoothingSpeed * Quaternion.Angle(transform.rotation, networkRotation) * Time.deltaTime * (1.0f / PhotonNetwork.SerializationRate));
    }
    public void SetAnimatorSequence(AnimatorSequence sequenceOrder)
    {
        currentAnimatorSequence = sequenceOrder;
    }
    public void SetDodge()
    {
        currentAnimatorSequence = AnimatorSequence.dodge;
    }

    void SyncAttacks()
    {
        AnimatorStateInfo currentStatInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimatorSequence == AnimatorSequence.none)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Attacking", false);
        } 
        // We are suppose to be in the first state
        else if(currentAnimatorSequence == AnimatorSequence.first)
        {
            // If we are not in the first state, go there
            if(!currentStatInfo.IsName("Attack"))
            {
                animator.Play("Base Layer.Attack");
            }
        }
        // We are suppose to be in the second state
        else if (currentAnimatorSequence == AnimatorSequence.second)
        {
            // If we're not...
            if(!currentStatInfo.IsName("Attack2"))
            {
                // Check that we are at least in the first attack state
                if(currentStatInfo.IsName("Attack"))
                {
                    animator.SetBool("Attack", true);
                }
                // We are not even in attack state 1, skip it and go to attackstate 2
                else
                {
                    animator.Play("Base Layer.Attack2");
                }
            }
        }
        // We are suppose to be in the third state
        else if (currentAnimatorSequence == AnimatorSequence.third)
        {
            // If we're not...
            if (!currentStatInfo.IsName("Attack3"))
            {
                // Check that we are at least in the second attack state
                if (currentStatInfo.IsName("Attack2"))
                {
                    animator.SetBool("Attack", true);
                }
                // We are not even in attack state 2, skip it and go directly to attackstate 3
                else
                {
                    animator.Play("Base Layer.Attack3");
                }
            }
        }
        // Dodging
        else if(currentAnimatorSequence == AnimatorSequence.dodge)
        {
            if(!currentStatInfo.IsName("Dodge")) animator.Play("Base Layer.Dodge");
        }
        // Counter
        else if (currentAnimatorSequence == AnimatorSequence.counter)
        {
            if (!currentStatInfo.IsName("Counter")) animator.Play("Base Layer.Counter");
        }
        // Counterattacking
        else if (currentAnimatorSequence == AnimatorSequence.counterAttack)
        {
            if (!currentStatInfo.IsName("CounterAttack")) animator.Play("Base Layer.CounterAttack");
        }
        // Sword clash
        else if (currentAnimatorSequence == AnimatorSequence.swordClash)
        {
            if (!currentStatInfo.IsName("SwordClash")) animator.Play("Base Layer.SwordClash");
        }
        // Air Attack
        else if (currentAnimatorSequence == AnimatorSequence.airAttack)
        {
            if (!currentStatInfo.IsName("AirAttack")) animator.Play("Base Layer.AirAttack");
        }
        // Sprint
        else if (currentAnimatorSequence == AnimatorSequence.sprint)
        {
            if (!currentStatInfo.IsName("Sprint")) animator.Play("Base Layer.Sprint");
        }
        // Charge
        else if (currentAnimatorSequence == AnimatorSequence.charge)
        {
            if (!currentStatInfo.IsName("Charge")) animator.Play("Base Layer.Charge");
        }
        // Level 2
        else if (currentAnimatorSequence == AnimatorSequence.level2)
        {
            if (!currentStatInfo.IsName("Level2")) animator.Play("Base Layer.Level2");
        }// Level 3
        else if (currentAnimatorSequence == AnimatorSequence.level3)
        {
            if (!currentStatInfo.IsName("Level3")) animator.Play("Base Layer.Level3");
        }

    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If this is owned by the client, we write data for all the other clients to receive
        if (stream.IsWriting)
        {
            if (syncRotation) stream.SendNext(transform.rotation.eulerAngles.y);
            if (syncAnimator) stream.SendNext(currentAnimatorSequence);
            if (syncHealth) stream.SendNext(healthScript.Health);

        }
        // If this isn't owned by the client we read data instead of writing
        else if (stream.IsReading)
        {
            if (syncRotation)
            {
                if (useSmoothing) networkRotation = Quaternion.Euler(new Vector3(0, (float)stream.ReceiveNext(), 0));
                else transform.rotation = Quaternion.Euler(new Vector3(0, (float)stream.ReceiveNext(), 0));
            }
            if (syncAnimator)
            {
                currentAnimatorSequence = (AnimatorSequence)stream.ReceiveNext();
                SyncAttacks();
            }
            if(syncHealth) healthScript.Health = (float)stream.ReceiveNext();
        }
    }
}
