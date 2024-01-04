using DG.Tweening;
using Managers;
using MyBox;
using UnityEngine;

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
                }
                return;
            }
        }

        public void SpawnRabbit()
        {
            if (ECManager.totalPoints >= rabbitCost)
            {
                ecManager.DecrementPoints(rabbitCost);
                Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);
            }
        }

        private void FindAliveRabbits()
        {
            GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
            amountOfRabbits = rabbits.Length;
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

