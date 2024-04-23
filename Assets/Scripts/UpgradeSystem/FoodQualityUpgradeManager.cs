using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FoodQualityUpgradeManager : MonoBehaviour
{
    public LeafPointManager leafPointManager;
    public FoodSpawner foodSpawner;
    public int[] upgradeCostAmount;
    public TextMeshProUGUI foodQualityUpgradeCostText;
    public Button foodQualityUpgradeButton;
    public GameObject maxUpgradeReachedText;
    public GameObject upgradeImage;
    public GameObject upgradeCostText;
    
    private int currentUpgradeCost;
    private int upgradeIndex = 0;

    private void Start() 
    {
        currentUpgradeCost = upgradeCostAmount[0];
        foodQualityUpgradeCostText.text = currentUpgradeCost.ToString();
    }

    public void IncreaseFoodQuality()
    {
        if (upgradeIndex < foodSpawner.foodPrefabs.Length)
        {
            currentUpgradeCost = upgradeCostAmount[upgradeIndex];
            if (LeafPointManager.totalPoints >= currentUpgradeCost)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                foodSpawner.foodPrefabsIndex++;
                LeafPointManager.totalPoints -= currentUpgradeCost;
                upgradeIndex++;
                
                if (upgradeIndex < upgradeCostAmount.Length) {
                    foodQualityUpgradeCostText.text = upgradeCostAmount[upgradeIndex].ToString();
                } else {
                    Debug.LogWarning("No more upgrades available.");
                    upgradeImage.SetActive(false);
                    upgradeCostText.SetActive(false);
                    maxUpgradeReachedText.SetActive(true); 
                    foodQualityUpgradeButton.interactable = false;
                }
            }
            else
            {
                Debug.LogWarning("Insufficient points to upgrade.");
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                leafPointManager.FlickerTotalPointsElement();
            }
        }
    }
}
