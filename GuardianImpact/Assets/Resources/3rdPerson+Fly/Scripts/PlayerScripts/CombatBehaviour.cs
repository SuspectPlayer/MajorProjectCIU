using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBehaviour : GenericBehaviour
{
    public Animator animator;
    public float mouseTimer;
    public bool isPressed;
    public bool chargeReady;
    void Start()
    {
        isPressed = false;
        animator = GetComponent<Animator>();
        chargeReady = true;
    }

    void Update()
    {
        // Start Mouse timer
        if (isPressed)
        {
            mouseTimer += 1 * Time.deltaTime;
        }

        // Counter attack
        if (Input.GetMouseButtonDown(1) && (behaviourManager.offlineMode || photonView.IsMine))
        {
            animator.Play("Base Layer.Counter");
            animator.SetBool("Attacking", true);
            Debug.Log("COUNTER ATTACK");
        }


        if (!animator.GetBool("Attacking"))
        {

            if (Input.GetMouseButtonDown(0) && (behaviourManager.offlineMode || photonView.IsMine))
            {
                isPressed = true;
                Debug.Log("Holding Mouse 1" + mouseTimer);

                // Air Focus Attack
                if (!behaviourManager.IsGrounded())
                {
                    animator.Play("Base Layer.AirAttack");
                }

                
            }
            // Start Charge Attack
            if (mouseTimer > .25f && !animator.GetBool("Attacking") && chargeReady)
            {
                animator.Play("Base Layer.Charge");
                chargeReady = false;
            }

            if (Input.GetMouseButtonUp(0) && !animator.GetBool("Attacking") && (behaviourManager.offlineMode || photonView.IsMine))
            {
                    if (mouseTimer < .25f && behaviourManager.IsGrounded())
                    {
                        animator.Play("Base Layer.Attack");
                        animator.SetBool("Attacking", true);
                        Debug.Log("NORMAL ATTACK");
                        isPressed = false;
                        mouseTimer = 0;
                    }

                    else if (mouseTimer > .25f && mouseTimer < 1f)
                    {

                        Debug.Log("LEVEL 1 ATTACK");
                        isPressed = false;
                        mouseTimer = 0;
                        chargeReady = true;
                        animator.Play("Base Layer.Attack3");
                    }
                    else if (mouseTimer > 1f && mouseTimer < 2f)
                    {
                        Debug.Log("LEVEL 2 ATTACK");
                        isPressed = false;
                        mouseTimer = 0;
                        chargeReady = true;
                        animator.Play("Base Layer.Level2");
                    }
                    else if (mouseTimer > 2f)
                    {
                        Debug.Log("LEVEL 3 ATTACK");
                        isPressed = false;
                        mouseTimer = 0;
                        chargeReady = true; 
                        animator.Play("Base Layer.Level3");
                    }
            }
            

        }

        
        



    }
}
