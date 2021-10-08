using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SlashControl : MonoBehaviour
{
    public GameObject normalAttack;
    public GameObject normalAttack2;
    public GameObject focusAttack;
    public GameObject focusAttackAir;
    public GameObject counterAttack;
    

    public void playNormalFX()
    {
        normalAttack.GetComponent<VisualEffect>().Play();
    }
    public void playNormalFX2()
    {
        normalAttack2.GetComponent<VisualEffect>().Play();
    }
    public void playFocusFX()
    {
        focusAttack.GetComponent<VisualEffect>().Play();
    }
    public void playAirFocusFX()
    {
        focusAttackAir.GetComponent<VisualEffect>().Play();
    }
    public void playCounterFX()
    {
        counterAttack.GetComponent<VisualEffect>().Play();
    }
}
