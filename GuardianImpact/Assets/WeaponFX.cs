using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    public ParticleSystem normalWeaponTrail;

    public void PlayWeaponFX()
    {
        normalWeaponTrail.Stop();
        if (normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }
    }
}
