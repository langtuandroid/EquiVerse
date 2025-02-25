using Managers;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UpgradeSystem
{
    public class MaxFoodUpgradeManager : MonoBehaviour
    {
        public GameManager gameManager;
        public LeafPointManager leafPointManager;
        public FoodSpawner foodSpawner;
        
        public int[] upgradeAmount;
        public GameObject maxPlantCostField;
        public Button maxPlantUpgradeButton;
        public TextMeshProUGUI maxPlantValueText;
        public TextMeshProUGUI maxPlantUpgradeCostText;

        private int currentUpgradeCost;
        private int upgradeIndex = 0;

        private bool tutorialStepCompleted = false;

        private void Start()
        {
            maxPlantValueText.text = (foodSpawner.maxPlants).ToString();
            currentUpgradeCost = upgradeAmount[0];
            maxPlantUpgradeCostText.text = currentUpgradeCost.ToString();
        }

        public void IncreaseMaxPlants() {
            if (upgradeIndex < upgradeAmount.Length) {
                currentUpgradeCost = upgradeAmount[upgradeIndex];

                if (LeafPointManager.totalPoints >= currentUpgradeCost) {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                    foodSpawner.maxPlants++;
                    maxPlantValueText.text = (foodSpawner.maxPlants).ToString();

                    LeafPointManager.totalPoints -= currentUpgradeCost;
                    upgradeIndex++;

                    if (!tutorialStepCompleted && gameManager.level2)
                    {
                        TutorialManager.CompleteStepAndContinueToNextStep("Step_MaxPlantUpgrade");
                        tutorialStepCompleted = true;
                    }
                    
                    if (!tutorialStepCompleted && gameManager.level3)
                    {
                        TutorialManager.CompleteStep("ShowPreviousUnlockedUpgrades");
                        TutorialManager.GoToNextStep();
                        tutorialStepCompleted = true;
                    }
                    if (!tutorialStepCompleted && (gameManager.level4 || gameManager.level5))
                    {
                        TutorialManager.CompleteStepAndContinueToNextStep("ShowFoodUpgrades");
                        tutorialStepCompleted = true;
                    }

                    if (upgradeIndex < upgradeAmount.Length) {
                        maxPlantUpgradeCostText.text = upgradeAmount[upgradeIndex].ToString();
                    } else {
                        Debug.LogWarning("No more upgrades available.");
                        maxPlantValueText.text = "MAX";
                        maxPlantCostField.SetActive(false);
                        maxPlantUpgradeButton.interactable = false;
                    }
                } else {
                    Debug.LogWarning("Insufficient points to upgrade.");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                    leafPointManager.FlickerTotalPointsElement();
                }
            } else {
                Debug.LogWarning("No more upgrades available.");
            }
        }
    }
}
