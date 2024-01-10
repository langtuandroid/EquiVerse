using System.Text.RegularExpressions;
using DG.Tweening;
using Managers;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Spawners {
    public class PlantSpawner : MonoBehaviour {
        private Camera mainCamera;

        [Header("Managers")]
        public GameManager gameManager;
        public ECManager ecManager;

        public LayerMask groundLayer;
        public GameObject grassPrefab;

        [Header("GrassCost")] public int grassCost = 20;

        [Header("Maximum plants")]
        public int maxPlants = 2; // Maximum number of allowed plants
        private static int currentPlantCount; // Track the number of plants in the scene
        public static bool canSpawnPlants = true;

        public int[] upgradeAmount;
        public TextMeshProUGUI maxPlantValueText;
        public TextMeshProUGUI maxPlantUpgradeCostText;
        public GameObject endOfLevelUpgrade;

        private int currentUpgradeCost;
        private int upgradeIndex = 0;

        [Header("GuidedTutorialSetup")]
        public bool isTutorial;
        [ConditionalField("isTutorial")]
        public GameObject maxPlantPopUp;
        [ConditionalField("isTutorial")]
        public GameObject tutorialStep;

        private void Start() {
            mainCamera = Camera.main;
            currentPlantCount = 0;

            maxPlantValueText.text = maxPlants.ToString();
            currentUpgradeCost = upgradeAmount[0];
            maxPlantUpgradeCostText.text = currentUpgradeCost.ToString();
        }

        public void ClickOnGround(Vector3 point) {
            if(!canSpawnPlants) return;
            // Check if the hit point is on the NavMesh
            if (IsPointOnNavMesh(point) && ECManager.totalPoints >= grassCost) {
                if (currentPlantCount < maxPlants) {
                    // Instantiate the grass prefab at the clicked position
                    GameObject spawnedPrefab = Instantiate(grassPrefab, point, Quaternion.identity);
                    spawnedPrefab.transform.DOScale(1f, 0.75f).SetEase(Ease.OutElastic);
                    ecManager.DecrementPoints(grassCost);
                    tutorialStep.SetActive(false);

                    currentPlantCount++; // Increment the plant count
                } else if (!maxPlantPopUp.activeInHierarchy && gameManager.tutorialActivated) {
                    maxPlantPopUp.SetActive(true);
                    PopInAnimation(maxPlantPopUp);
                }
            }
        }

        // Check if a given point is on the NavMesh
        bool IsPointOnNavMesh(Vector3 point) {
            NavMeshHit hit;
            return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
        }

        // You should call this method when a plant is removed or destroyed.
        public static void RemovePlant() {
            if (currentPlantCount > 0) {
                currentPlantCount--; // Decrement the plant count
            }
        }

        public void IncreaseMaxPlants() {
            if (upgradeIndex < upgradeAmount.Length - 1) {
                currentUpgradeCost = upgradeAmount[upgradeIndex];

                if (ECManager.totalPoints >= currentUpgradeCost) {
                    maxPlants++;
                    maxPlantValueText.text = maxPlants.ToString();

                    ECManager.totalPoints -= currentUpgradeCost;
                    upgradeIndex++;

                    // Check if upgradeIndex is still within bounds before accessing the array
                    if (upgradeIndex < upgradeAmount.Length - 1) {
                        maxPlantUpgradeCostText.text = upgradeAmount[upgradeIndex].ToString();
                    } else {
                        Debug.LogWarning("No more upgrades available.");
                        // You can choose to disable the upgrade button or handle it according to your game logic.
                    }

                    if (gameManager.secondLevelTutorialActivated && upgradeIndex == 1) {
                        endOfLevelUpgrade.SetActive(true);
                    }
                } else {
                    Debug.LogWarning("Insufficient points to upgrade.");
                }
            } else {
                Debug.LogWarning("No more upgrades available.");
                // You can choose to disable the upgrade button or handle it according to your game logic.
            }
        }



        private void PopInAnimation(GameObject gameObject) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null) {
                rectTransform.localScale = new Vector3(0f, 0f, 0f);
                rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            }
        }

        bool IsCollectable(GameObject obj) {
            // Replace "CollectableLayer" with the actual layer of your collectable objects
            return obj.layer == LayerMask.NameToLayer("CollectableLayer");
        }
    }
}
