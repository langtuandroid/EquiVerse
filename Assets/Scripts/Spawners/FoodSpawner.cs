using System.Text.RegularExpressions;
using DG.Tweening;
using Managers;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Spawners {
    public class FoodSpawner : MonoBehaviour {
        
        [Header("Managers")]
        public GameManager gameManager;
        public LeafPointManager leafPointManager;

        public NavMeshSurface navMeshSurface; 
        public GameObject[] foodPrefabs;
        [HideInInspector]
        public int foodPrefabsIndex = 0;

        [Header("GrassCost")] 
        public int grassCost;

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

        public void ClickOnGround(Vector3 point) 
        {
            if (!CanSpawnPlants) 
                return;

            point = new Vector3(point.x, 0.5f, point.z); // Adjust spawn height to prevent plants from spawning too high

            if (IsPointOnNavMesh(point)) 
            {
                if (LeafPointManager.totalPoints >= grassCost) 
                {
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
                        TutorialManager.CompleteStepAndContinueToNextStep("Step_GrassSpawn");
                        currentPlantCount++;
                        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/GrassPlacement");
                    }
                    else if (!maxFoodPopup.activeInHierarchy && gameManager.level1 && timesPopupShown < 3) 
                    {
                        maxFoodPopup.SetActive(true);
                        PopInAnimation(maxFoodPopup);
                        timesPopupShown++;
                        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/PopupWarning");
                        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                    }
                }
                else 
                {
                    Debug.LogWarning("Insufficient points to buy plant.");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                    leafPointManager.FlickerTotalPointsElement();
                }
            }
        }

        bool IsPointOnNavMesh(Vector3 point) {
            NavMeshHit hit;
            //TODO: Make this break proof please
            return NavMesh.SamplePosition(point, out hit, 0.1f, new NavMeshQueryFilter{agentTypeID = NavMesh.GetSettingsByIndex(3).agentTypeID, areaMask = NavMesh.AllAreas});
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
