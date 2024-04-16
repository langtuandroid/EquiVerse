using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GunUpgrade
{
    public ParticleSystem gunParticles;
    public FMODUnity.EventReference gunImpactSoundEventPath;
    public int gunDamage;
}

public class GunUpgradeManager : MonoBehaviour
{
    private static GunUpgradeManager _instance;
    public List<GunUpgrade> gunUpgrades = new List<GunUpgrade>();
    public LeafPointManager leafPointManager;
    
    public Button weaponUpgradeUpgradeButton;
    public GameObject maxUpgradeReachedText;
    public TextMeshProUGUI weaponUpgradeCostText;
    public GameObject upgradeCostText;
    public int[] upgradeCostAmount;
    public GameObject upgradeImage;
    private int gunUpgradeIndex;
    private int currentUpgradeCost;

    private bool gunTutorialStepCompleted = false;
    

    private void Start()
    {
        gunUpgradeIndex = 0;
        currentUpgradeCost = upgradeCostAmount[0];
        weaponUpgradeCostText.text = currentUpgradeCost.ToString();
        _instance = this;
    }

    public void UpgradeGun()
    {
        if (gunUpgradeIndex <= gunUpgrades.Count)
        {
            currentUpgradeCost = upgradeCostAmount[gunUpgradeIndex];
            if (LeafPointManager.totalPoints >= currentUpgradeCost)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                if (!gunTutorialStepCompleted)
                {
                    TutorialManager.CompleteStepAndContinueToNextStep("ShowUpgrades");
                    gunTutorialStepCompleted = true;
                }
                LeafPointManager.totalPoints -= currentUpgradeCost;
                gunUpgradeIndex++;
                
                if (gunUpgradeIndex < upgradeCostAmount.Length) {
                    weaponUpgradeCostText.text = upgradeCostAmount[gunUpgradeIndex].ToString();
                } else {
                    Debug.LogWarning("No more upgrades available.");
                    upgradeImage.SetActive(false);
                    upgradeCostText.SetActive(false);
                    maxUpgradeReachedText.SetActive(true); 
                    weaponUpgradeUpgradeButton.interactable = false;
                }
            }
        } else
        {
            Debug.LogWarning("Insufficient points to upgrade.");
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
            leafPointManager.FlickerTotalPointsElement();
        }
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