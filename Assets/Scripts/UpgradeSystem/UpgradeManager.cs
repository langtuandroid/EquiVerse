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

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;
    public List<GunUpgrade> gunUpgrades = new List<GunUpgrade>();

    private int gunUpgradeIndex = 0;

    private void Start()
    {
        _instance = this;
    }

    public static UpgradeManager GetInstance()
    {
        return _instance;
    }
    public GunUpgrade GetCurrentGunUpgrade()
    {
        return gunUpgrades[gunUpgradeIndex];
    }
}