using System;
using System.Numerics;
using DG.Tweening;
using Managers;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Spawners
{
    public class FoxSpawner : MonoBehaviour
    {
        [Header("Managers")] 
        public GameManager gameManager;
        public LeafPointManager leafPointManager;
        
        [Header("SpawnPrefabs")]
        public GameObject foxPrefab;
        public ParticleSystem spawnParticles;
        private Vector3 spawnPosition;

        [Header("SpawnCost")]
        public int foxCost = 1000;

        public Transform spawnLocation;

        private int amountOfFoxes;
        private bool foxStepCompleted;
        
        private void Start()
        {
            spawnPosition = new Vector3(spawnLocation.transform.position.x/2,1f, spawnLocation.transform.position.z/2);
        }

        public void SpawnFox()
        {
            if (LeafPointManager.totalPoints >= foxCost)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                leafPointManager.DecrementPoints(foxCost);
                GameObject foxInstance = Instantiate(foxPrefab, spawnPosition, Quaternion.identity);
                spawnParticles.Play();
                foxInstance.transform.localScale = Vector3.zero;
                foxInstance.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
                FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/SpawnAnimal");
                
                if (!foxStepCompleted && gameManager.level3)
                {
                    TutorialManager.CompleteStepAndContinueToNextStep("ShowFoxButton");
                    foxStepCompleted = true;
                }
                
                if (!foxStepCompleted && gameManager.level4)
                {
                    TutorialManager.CompleteStepAndContinueToNextStep("ShowFoxUpgrade");
                    foxStepCompleted = true;
                }
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                leafPointManager.FlickerTotalPointsElement();
            }
        }

        public void FindAliveFoxes()
        {
            amountOfFoxes = EntityManager.Get().GetFoxes().Count;
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

