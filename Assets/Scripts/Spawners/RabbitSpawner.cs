using UnityEngine;
using DG.Tweening;
using Managers;

namespace Spawners
{
    public class RabbitSpawner : MonoBehaviour
    {
        [Header("Managers")] 
        public GameManager gameManager;
        public LeafPointManager leafPointManager;
        
        [Header("Spawner")]
        public GameObject rabbitPrefab;
        public int rabbitCost = 100;
        public Transform spawnLocation;

        public GameObject gameOverPopUp;
        
        private Vector3 spawnPosition;
        private int amountOfRabbits;
        private bool finishLevelStepCompleted = false;
        private bool showUpgrades = false;

        private void Start()
        {
            spawnPosition = new Vector3(spawnLocation.transform.position.x / 2, 1f, spawnLocation.transform.position.z / 2);
        }

        private void FixedUpdate()
        {
            if (LeafPointManager.totalPoints < rabbitCost)
            {
                FindAliveRabbits();
                if (amountOfRabbits <= 0)
                {
                    ShowGameOverPopUp();
                    PlaySound("event:/UI/PopupWarning");
                    PlaySound("event:/UI/OpeningUIElement");
                }
            }
        }

        public void SpawnRabbit()
        {
            if (LeafPointManager.totalPoints >= rabbitCost)
            {
                PlaySound("event:/UI/Buy");
                leafPointManager.DecrementPoints(rabbitCost);
                GameObject rabbitInstance = Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);
                rabbitInstance.transform.localScale = Vector3.zero;
                rabbitInstance.transform.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutBack);
                PlaySound("event:/PlayerActions/SpawnAnimal");
                
                FindAliveRabbits();
                
                if (amountOfRabbits == 2 && !finishLevelStepCompleted && gameManager.level1)
                {
                    TutorialManager.GoToNextStep();
                    finishLevelStepCompleted = true;
                }

                if (amountOfRabbits == 1 && !showUpgrades)
                {
                    TutorialManager.CompleteStepAndContinueToNextStep("StartGame");
                    showUpgrades = true;
                }
            }
            else
            {
                PlaySound("event:/UI/CantBuy");
                leafPointManager.FlickerTotalPointsElement();
            }
        }

        private void FindAliveRabbits()
        {
            amountOfRabbits = EntityManager.Get().GetRabbits().Count;
        }

        private void ShowGameOverPopUp()
        {
            gameOverPopUp.SetActive(true);
            PopInAnimation(gameOverPopUp);
            gameObject.SetActive(false);
        }

        private void PopInAnimation(GameObject obj)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.zero;
                rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            }
        }

        private void PlaySound(string eventName)
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventName);
        }
    }
}
