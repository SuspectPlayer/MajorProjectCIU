using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SlashControl : MonoBehaviour
{
    // White slash FX
    public GameObject normalLeftHorizontal;
    public GameObject normalRightHorizontal;
    public GameObject normalLeftDiagonalUp;
    public GameObject normalRightDiagonalUp;
    public GameObject normalLeftDiagonalDown;
    public GameObject normalRightDiagonalDown;
    // Blue slash FX
    public GameObject focusLeftHorizontal;
    public GameObject focusRightHorizontal;
    public GameObject focusLeftDiagonalUp;
    public GameObject focusRightDiagonalUp;
    public GameObject focusLeftDiagonalDown;
    public GameObject focusRightDiagonalDown;
    // Red slash FX
    public GameObject counterLeftHorizontal;
    public GameObject counterRightHorizontal;
    public GameObject counterLeftDiagonalUp;
    public GameObject counterRightDiagonalUp;
    public GameObject counterLeftDiagonalDown;
    public GameObject counterRightDiagonalDown;
    public void NLH()
    {
        normalLeftHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void NRH()
    {
        normalRightHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void NLDU()
    {
        normalLeftDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void NRDU()
    {
        normalRightDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void NLDD()
    {
        normalLeftDiagonalDown.GetComponent<VisualEffect>().Play();
    }
    public void NRDD()
    {
        normalRightDiagonalDown.GetComponent<VisualEffect>().Play();
    }

    public void FLH()
    {
        focusLeftHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void FRH()
    {
        focusRightHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void FLDU()
    {
        focusLeftDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void FRDU()
    {
        focusRightDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void FLDD()
    {
        focusLeftDiagonalDown.GetComponent<VisualEffect>().Play();
    }
    public void FRDD()
    {
        focusRightDiagonalDown.GetComponent<VisualEffect>().Play();
    }

    public void CLH()
    {
        counterLeftHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void CRH()
    {
        counterRightHorizontal.GetComponent<VisualEffect>().Play();
    }
    public void CLDU()
    {
        counterLeftDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void CRDU()
    {
        counterRightDiagonalUp.GetComponent<VisualEffect>().Play();
    }
    public void CLDD()
    {
        counterLeftDiagonalDown.GetComponent<VisualEffect>().Play();
    }
    public void CRDD()
    {
        counterRightDiagonalDown.GetComponent<VisualEffect>().Play();
    }
}
