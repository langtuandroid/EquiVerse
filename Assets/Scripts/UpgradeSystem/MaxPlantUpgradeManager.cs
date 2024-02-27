using Managers;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem
{
    public class MaxPlantUpgradeManager : MonoBehaviour
    {
        public GameManager gameManager;
        public ECManager ecManager;
        public PlantSpawner plantSpawner;
        
        public int[] upgradeAmount;
        public GameObject maxPlantCostField;
        public Button maxPlantUpgradeButton;
        public TextMeshProUGUI maxPlantValueText;
        public TextMeshProUGUI maxPlantUpgradeCostText;
        public GameObject endOfLevelUpgrade;

        private int currentUpgradeCost;
        private int upgradeIndex = 0;

        private void Start()
        {
            maxPlantValueText.text = (plantSpawner.maxPlants + 1).ToString();
            currentUpgradeCost = upgradeAmount[0];
            maxPlantUpgradeCostText.text = currentUpgradeCost.ToString();
        }

        public void IncreaseMaxPlants() {
            if (upgradeIndex < upgradeAmount.Length) {
                currentUpgradeCost = upgradeAmount[upgradeIndex];

                if (ECManager.totalPoints >= currentUpgradeCost) {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                    plantSpawner.maxPlants++;
                    maxPlantValueText.text = (plantSpawner.maxPlants + 1).ToString();

                    ECManager.totalPoints -= currentUpgradeCost;
                    upgradeIndex++;

                    if (upgradeIndex < upgradeAmount.Length) {
                        maxPlantUpgradeCostText.text = upgradeAmount[upgradeIndex].ToString();
                    } else {
                        Debug.LogWarning("No more upgrades available.");
                        maxPlantValueText.text = "MAX";
                        maxPlantCostField.SetActive(false);
                        maxPlantUpgradeButton.interactable = false;
                    }

                    if (gameManager.secondLevelTutorialActivated && upgradeIndex == 1) {
                        endOfLevelUpgrade.SetActive(true);
                    }
                } else {
                    Debug.LogWarning("Insufficient points to upgrade.");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                    ecManager.FlickerTotalPointsElement();
                }
            } else {
                Debug.LogWarning("No more upgrades available.");
            }
        }
    }
}
