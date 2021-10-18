using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SlashControl : MonoBehaviour
{
    public GameObject damageSphere;
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

    private void Start()
    {
        damageSphere.SetActive(false);
    }

    public void damageSphereOn()
    {
        if (!damageSphere.activeSelf)
        {
            damageSphere.SetActive(true);
        }
    }
    public void damageSphereOff()
    {
        if (damageSphere.activeSelf)
        {
            damageSphere.SetActive(false);
        }
    }
    public void NLH()
    {
        normalLeftHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void NRH()
    {
        normalRightHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void NLDU()
    {
        normalLeftDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void NRDU()
    {
        normalRightDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void NLDD()
    {
        normalLeftDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void NRDD()
    {
        normalRightDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }

    public void FLH()
    {
        focusLeftHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void FRH()
    {
        focusRightHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void FLDU()
    {
        focusLeftDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void FRDU()
    {
        focusRightDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void FLDD()
    {
        focusLeftDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void FRDD()
    {
        focusRightDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }

    public void CLH()
    {
        counterLeftHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void CRH()
    {
        counterRightHorizontal.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void CLDU()
    {
        counterLeftDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void CRDU()
    {
        counterRightDiagonalUp.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void CLDD()
    {
        counterLeftDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
    public void CRDD()
    {
        counterRightDiagonalDown.GetComponent<VisualEffect>().Play(); damageSphereOn();
    }
}
