using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem instance;
    StarterAssets.StarterAssetsInputs inputs;
    public bool canInput;
    public bool gotInput;

    void Awake()
    {
        instance = this;
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    //    private void OnEnable()
    //    {
    //        actions.Enable();
    //        actions.Player.Attack1.performed += Attack1;
    //        actions.Player.Attack2.performed += Attack2;
    //        actions.Player.Attack3.performed += Attack3;
    //}

    //    private void OnDisable()
    //    {
    //        actions.Player.Disable();
    //    }

    //    void Attack1(InputAction.CallbackContext context)
    //    {
    //    Debug.Log("Attack1 called");
    //        if (context.performed)
    //        {
    //            if (canInput)
    //            {
    //                gotInput = true;
    //                canInput = false;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }

    //    }
    //    void Attack2(InputAction.CallbackContext context)
    //    {

    //    Debug.Log("Attack2 called");
    //    if (context.performed)
    //        {
    //            if (canInput)
    //            {
    //                gotInput = true;
    //                canInput = false;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }

    //    }
    //    void Attack3(InputAction.CallbackContext context)
    //    {

    //    Debug.Log("Attack3 called");
    //    if (context.performed)
    //        {
    //            if (canInput)
    //            {
    //                gotInput = true;
    //                canInput = false;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }

    //    }

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
