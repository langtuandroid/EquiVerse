using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunUpgrade
{
    public ParticleSystem gunParticles;
    public int gunDamage;
}

public class GunUpgradeManager : MonoBehaviour
{
    private static GunUpgradeManager _instance;
    public List<GunUpgrade> gunUpgrades = new List<GunUpgrade>();

    private int gunUpgradeIndex = 0;

    private void Start()
    {
        _instance = this;
    }

    public static GunUpgradeManager GetInstance()
    {
        return _instance;
    }
    public GunUpgrade GetCurrentGunUpgrade()
    {
        return gunUpgrades[gunUpgradeIndex];
    }
}