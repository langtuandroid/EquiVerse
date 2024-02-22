using System.Numerics;
using DG.Tweening;
using Managers;
using MyBox;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Spawners
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Managers")] 
        public GameManager gameManager;
        public ECManager ecManager;
        
        [Header("SpawnPrefabs")]
        public GameObject rabbitPrefab;

        private Vector3 spawnPosition;

        [Header("SpawnCost")]
        public int rabbitCost = 100;

        public Transform spawnLocation;

        private int amountOfRabbits;
        
        [Header("GuidedTutorialSetup")]
        public bool isTutorial;
        [ConditionalField("isTutorial")]
        public GameObject gameOverPopUp;
        
        public bool isSecondLevel;
        [ConditionalField("isSecondLevel")]
        public SecondLevelTutorial secondLevelTutorial;

        private void Start()
        {
            spawnPosition = new Vector3(spawnLocation.transform.position.x/2,0.5f, spawnLocation.transform.position.z/2);
        }

        private void FixedUpdate()
        {
            if (ECManager.totalPoints < rabbitCost)
            {
                FindAliveRabbits();
                if (amountOfRabbits <= 0 && gameManager.tutorialActivated)
                {
                    ShowGameOverPopUp();
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/PopupWarning");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                }
                return;
            }
        }

        public void SpawnRabbit()
        {
            if (ECManager.totalPoints >= rabbitCost)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                ecManager.DecrementPoints(rabbitCost);
                GameObject rabbitInstance = Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);
                rabbitInstance.transform.localScale = Vector3.zero;
                rabbitInstance.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
                FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/SpawnAnimal");
                
                if (gameManager.secondLevelTutorialActivated)
                {
                    //secondLevelTutorial.ShowMaxPlantUpgradeButton();
                }
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                ecManager.FlickerTotalPointsElement();
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
    }
}

