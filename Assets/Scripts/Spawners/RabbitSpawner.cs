using System.Numerics;
using DG.Tweening;
using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Spawners
{
    public class RabbitSpawner : MonoBehaviour
    {
        [Header("Managers")] 
        public GameManager gameManager;
        [FormerlySerializedAs("ecManager")] public LeafPointManager leafPointManager;
        
        [Header("SpawnPrefabs")]
        public GameObject rabbitPrefab;

        private Vector3 spawnPosition;

        [Header("SpawnCost")]
        public int rabbitCost = 100;

        public Transform spawnLocation;

        private int amountOfRabbits;
        
        [Header("GuidedTutorialSetup")]
        private bool isTutorial = false;
        [ConditionalField("isTutorial")]
        public GameObject gameOverPopUp;
        
        private bool finishLevelStepCompleted = false;
        
        private void Start()
        {
            spawnPosition = new Vector3(spawnLocation.transform.position.x/2,0.5f, spawnLocation.transform.position.z/2);
        }

        private void FixedUpdate()
        {
            if (LeafPointManager.totalPoints < rabbitCost)
            {
                FindAliveRabbits();
                if (amountOfRabbits <= 0)
                {
                    if (gameManager.tutorialActivated)
                    {
                        ShowGameOverPopUp();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/PopupWarning");
                        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                    }
                }
            }
        }

        public void SpawnRabbit()
        {
            if (LeafPointManager.totalPoints >= rabbitCost)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                leafPointManager.DecrementPoints(rabbitCost);
                GameObject rabbitInstance = Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);
                rabbitInstance.transform.localScale = Vector3.zero;
                rabbitInstance.transform.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutBack);
                FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/SpawnAnimal");
                
                FindAliveRabbits();
                
                if (amountOfRabbits >= 4 && !finishLevelStepCompleted)
                {
                    TutorialManager.GoToNextStep();
                    finishLevelStepCompleted = true;
                }
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
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

        private void PopInAnimation(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.zero;
                rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            }
        }
        
        public void FinishLevelStep(bool _finishLevelStepCompleted)
        {
            finishLevelStepCompleted = _finishLevelStepCompleted;
        }
    }
}

