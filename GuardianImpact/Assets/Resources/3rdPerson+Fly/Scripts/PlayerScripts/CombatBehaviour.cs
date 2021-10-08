using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBehaviour : GenericBehaviour
{
    public Animator animator;
    public float mouseTimer;
    public bool isPressed;
    void Start()
    {
        isPressed = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift) && (behaviourManager.offlineMode || photonView.IsMine))
        //{
        //    animator.Play("Base Layer.Dodge");
        //    Debug.Log("DODGED");
        //}
        // Read mouse input and send whether player pressed or held the attack button
        if (!animator.GetBool("Attacking"))
        {
            if (Input.GetMouseButtonDown(0) && (behaviourManager.offlineMode || photonView.IsMine))
            {
                isPressed = true;
                Debug.Log("Holding Mouse 1" + mouseTimer);
                if (!behaviourManager.IsGrounded())
                {
                    animator.Play("Base Layer.AirAttack");
                }
            }

            if (mouseTimer > 1f && !animator.GetBool("Attacking"))
            {
                animator.Play("Base Layer.Charge");
            }

            if (Input.GetMouseButtonUp(0) && !animator.GetBool("Attacking") && (behaviourManager.offlineMode || photonView.IsMine))
            {
                if (mouseTimer < 1f && behaviourManager.IsGrounded())
                    animator.Play("Base Layer.Attack");
                animator.SetBool("Attacking", true);
                Debug.Log("NORMAL ATTACK");
                isPressed = false;
                mouseTimer = 0;
            }
            else if (mouseTimer > 1f && mouseTimer < 2f)
            {

                Debug.Log("LEVEL 1 ATTACK");
                isPressed = false;
                mouseTimer = 0;
                animator.SetBool("Level1", true);
            }
            else if (mouseTimer > 2f && mouseTimer < 3f)
            {
                Debug.Log("LEVEL 2 ATTACK");
                isPressed = false;
                mouseTimer = 0;
                animator.SetBool("Level2", true);
            }
            else if (mouseTimer > 3f)
            {
                Debug.Log("LEVEL 3 ATTACK");
                isPressed = false;
                mouseTimer = 0;
                animator.SetBool("Level3", true);
            }

        }

        if (isPressed)
        {
            mouseTimer += 1 * Time.deltaTime;


        }
        if (Input.GetMouseButtonDown(1) && (behaviourManager.offlineMode || photonView.IsMine))
        {
            animator.Play("Base Layer.Counter");
            animator.SetBool("Attacking", true);
            Debug.Log("COUNTER ATTACK");
            // animator reset behaviour needed
        }



    }
}
