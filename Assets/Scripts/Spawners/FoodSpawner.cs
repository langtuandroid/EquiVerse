using System.Text.RegularExpressions;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Spawners {
    public class FoodSpawner : MonoBehaviour {
        [Header("Managers")]
        public GameManager gameManager;
        [FormerlySerializedAs("ecManager")] public LeafPointManager leafPointManager;

        public LayerMask groundLayer;
        public GameObject[] foodPrefabs;
        [HideInInspector]
        public int foodPrefabsIndex = 0;

        [Header("GrassCost")] public int grassCost;

        [Header("Maximum plants")]
        public int maxPlants = 2;
        private int currentPlantCount;
        public bool CanSpawnPlants { get; set; }

        public GameObject maxFoodPopup;

        private int timesPopupShown;

        private void Start() {
            currentPlantCount = 0;
            timesPopupShown = 0;
        }

        public void ClickOnGround(Vector3 point) {
            if (!CanSpawnPlants) return;
            point = new Vector3(point.x, 0.5f, point.z); //Hackerman fix for plants spawning too high. Should work until there's a level where the ground has a different height.
            if (IsPointOnNavMesh(point) && LeafPointManager.totalPoints >= grassCost) {
                if (currentPlantCount < maxPlants)
                {
                    GameObject spawnedPrefab = Instantiate(foodPrefabs[foodPrefabsIndex], point, Quaternion.identity);
                    float randomYRotation = Random.Range(0f, 360f);
                    spawnedPrefab.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);
                    
                    switch (foodPrefabsIndex)
                    {
                        case 0:
                            spawnedPrefab.transform.DOScale(0.75f, 0.75f).SetEase(Ease.OutElastic);
                            break;
                        case 1:
                            spawnedPrefab.transform.DOScale(0.5f, 0.75f).SetEase(Ease.OutElastic);
                            break;
                        case 2:
                            spawnedPrefab.transform.DOScale(1.2f, 0.75f).SetEase(Ease.OutElastic);
                            break;
                    }

                    leafPointManager.DecrementPoints(grassCost);
                    TutorialManager.CompleteStepAndContinueToNextStep("Step_GrassSpawn2");

                    currentPlantCount++;

                    FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/GrassPlacement");
                } else if (!maxFoodPopup.activeInHierarchy && gameManager.tutorialActivated && timesPopupShown < 3) {
                    maxFoodPopup.SetActive(true);
                    PopInAnimation(maxFoodPopup);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/PopupWarning");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                    timesPopupShown++;
                }
            }
        }

        bool IsPointOnNavMesh(Vector3 point) {
            NavMeshHit hit;
            return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
        }

        public void RemovePlant() {
            if (currentPlantCount > 0) {
                currentPlantCount--;
            }
        }

        private void PopInAnimation(GameObject gameObject) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null) {
                rectTransform.localScale = new Vector3(0f, 0f, 0f);
                rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            }
        }
    }
}
