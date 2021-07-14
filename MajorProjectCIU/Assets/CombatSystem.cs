using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem instance;

    public StarterAssetsInputs combat;

    public bool canInput;
    public bool gotInput;

    void Awake()
    {
        instance = this;
        combat = instance.GetComponent<StarterAssetsInputs>();
        
    }
    private void Update()
    {
        if (combat.attack1)
        {
            Attack1();
        }
    }

    void Attack1()
    {
        
            Debug.Log("Attack1 called");
            if (canInput)
            {
                gotInput = true;
                canInput = false;
            }
            else
            {
                return;
            }
        
    }
    void Attack2(InputAction.CallbackContext context)
    {


        if (context.performed)
        {
            Debug.Log("Attack2 called");
            if (canInput)
            {
                gotInput = true;
                canInput = false;
            }
            else
            {
                return;
            }
        }

    }
    void Attack3(InputAction.CallbackContext context)
    {


        if (context.performed)
        {
            Debug.Log("Attack3 called");
            if (canInput)
            {
                gotInput = true;
                canInput = false;
            }
            else
            {
                return;
            }
        }

    }

    public void InputManager()
    {
        if (!canInput)
        {
            canInput = true;
        }
        else
        {
            canInput = false;
        }
    }
}
