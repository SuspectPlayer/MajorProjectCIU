using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    
    [SerializeField] bool syncRotation = false;
    [SerializeField] bool useSmoothing = true;
    [SerializeField] bool syncAnimator = true;
    [SerializeField] float smoothingSpeed = 130f;
    Vector3 networkPosition;
    Quaternion networkRotation;

    AttackSequence currentSequence;

    Animator animator;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.gameObject;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!useSmoothing || photonView.IsMine) return;
        if (syncRotation) transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, smoothingSpeed * Quaternion.Angle(transform.rotation, networkRotation) * Time.deltaTime * (1.0f / PhotonNetwork.SerializationRate));
    }
    public void SetAttackSequence(AttackSequence sequenceOrder)
    {
        currentSequence = sequenceOrder;
    }

    void SyncAttacks()
    {
        AnimatorStateInfo currentStatInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (currentSequence == AttackSequence.none)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Attacking", false);
        } 
        // We are suppose to be in the first state
        else if(currentSequence == AttackSequence.first)
        {
            // If we are not in the first state, go there
            if(!currentStatInfo.IsName("Attack"))
            {
                animator.Play("Base Layer.Attack");
            }
        }
        // We are suppose to be in the second state
        else if (currentSequence == AttackSequence.second)
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
        else if (currentSequence == AttackSequence.third)
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
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If this is owned by the client, we write data for all the other clients to receive
        if (stream.IsWriting)
        {
            if (syncRotation) stream.SendNext(transform.rotation.eulerAngles.y);
            if (syncAnimator)
            {
                stream.SendNext(currentSequence);
            }
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
                currentSequence = (AttackSequence)stream.ReceiveNext();
                SyncAttacks();

            }
        }
    }
}
