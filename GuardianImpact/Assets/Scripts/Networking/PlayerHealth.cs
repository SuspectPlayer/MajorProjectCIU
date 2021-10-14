using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    [SerializeField] float health = 100f;
    [SerializeField] float maxHealth = 100f;
    public float Health { get { return health; } internal set { health = value; } }

    public void TakeDamage(float value)
    {
        health -= value;
    }
}
