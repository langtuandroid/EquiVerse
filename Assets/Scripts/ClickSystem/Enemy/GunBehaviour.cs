using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public static int gunDamage;
    private int baseGunDamage = 10;

    void Start()
    {
        gunDamage = baseGunDamage;
    }
}
