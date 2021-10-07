using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSelect : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKey(KeyCode.W))
        {
            BasicBehaviour BB = animator.GetComponent<BasicBehaviour>();
            if (BB.offlineMode || BB.photonView.IsMine) animator.SetBool("W",true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            BasicBehaviour BB = animator.GetComponent<BasicBehaviour>();
            if (BB.offlineMode || BB.photonView.IsMine) animator.SetBool("A", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            BasicBehaviour BB = animator.GetComponent<BasicBehaviour>();
            if (BB.offlineMode || BB.photonView.IsMine) animator.SetBool("S", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            BasicBehaviour BB = animator.GetComponent<BasicBehaviour>();
            if (BB.offlineMode || BB.photonView.IsMine) animator.SetBool("D", true);
        }
        else
        {
            BasicBehaviour BB = animator.GetComponent<BasicBehaviour>();
            if (BB.offlineMode || BB.photonView.IsMine) Debug.Log("No Direction Pressed");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
