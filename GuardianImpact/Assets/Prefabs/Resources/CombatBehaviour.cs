using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBehaviour : GenericBehaviour
{
    private int attackBool;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        attackBool = Animator.StringToHash("Attacking");
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviourManager.IsGrounded())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!attacking)
                {
                    attacking = true;
                    behaviourManager.GetAnim.SetTrigger(attackBool);
                    Debug.Log("Hey");
                }
            }
        }
    }
}
