using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    
    [SerializeField] bool syncPosition = true;
    [SerializeField] bool syncRotation = false;
    [SerializeField] bool useSmoothing = true;
    [SerializeField] bool syncAnimator = true;
    [SerializeField] float smoothingSpeed = 130f;
    Vector3 networkPosition;
    Quaternion networkRotation;

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
        if (syncPosition) transform.position = Vector3.MoveTowards(transform.localPosition, networkPosition, smoothingSpeed * Vector3.Distance(transform.position, networkPosition) * Time.deltaTime * (1.0f / PhotonNetwork.SerializationRate));
        if (syncRotation) transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, smoothingSpeed * Quaternion.Angle(transform.rotation, networkRotation) * Time.deltaTime * (1.0f / PhotonNetwork.SerializationRate));
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If this is owned by the client, we write data for all the other clients to receive
        if (stream.IsWriting)
        {
            if (syncPosition) stream.SendNext(transform.position);
            if (syncRotation) stream.SendNext(transform.rotation.eulerAngles.y);
            if (syncAnimator)
            {
                stream.SendNext(animator.GetFloat("Speed"));
                /*stream.SendNext(animator.GetFloat("H"));
                stream.SendNext(animator.GetFloat("V"));*/
                
                stream.SendNext(animator.GetBool("Jump"));
                stream.SendNext(animator.GetBool("Fly"));
                stream.SendNext(animator.GetBool("Aim"));
                stream.SendNext(animator.GetBool("Grounded"));
            }
        }
        // If this isn't owned by the client we read data instead of writing
        else if (stream.IsReading)
        {
            if (syncPosition)
            {
                if (useSmoothing) networkPosition = (Vector3)stream.ReceiveNext();
                else transform.position = (Vector3)stream.ReceiveNext();
            }
            if (syncRotation)
            {
                if (useSmoothing) networkRotation = Quaternion.Euler(new Vector3(0, (float)stream.ReceiveNext(), 0));
                else transform.rotation = Quaternion.Euler(new Vector3(0, (float)stream.ReceiveNext(), 0));
            }
            if (syncAnimator)
            {
                animator.SetFloat("Speed", (float)stream.ReceiveNext());
                /*animator.SetFloat("H", (float)stream.ReceiveNext());
                animator.SetFloat("V", (float)stream.ReceiveNext());*/

                animator.SetBool("Jump", (bool) stream.ReceiveNext());
                animator.SetBool("Fly", (bool) stream.ReceiveNext());
                animator.SetBool("Aim", (bool) stream.ReceiveNext());
                animator.SetBool("Grounded", (bool) stream.ReceiveNext());
            }
        }
    }
}
