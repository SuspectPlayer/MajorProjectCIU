using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class Attack2 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<ThirdPersonController>().canAttack = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<ThirdPersonController>()._input.attack3)
        {
            animator.SetTrigger("Attack3");
            Debug.Log("Attacked");
            animator.GetComponent<ThirdPersonController>()._input.attack3 = false;
        }
        if (animator.GetComponent<ThirdPersonController>()._input.attack2)
        {
            animator.SetTrigger("Attack2");
            Debug.Log("Attacked");
            animator.GetComponent<ThirdPersonController>()._input.attack2 = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<ThirdPersonController>().canAttack = false;
    }

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
