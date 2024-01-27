using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers {
    public class ECManager : MonoBehaviour {
        public TextMeshProUGUI totalPointsText;
        public TextMeshProUGUI endOfLevelCostText;
        public Slider slider;
        public static int totalPoints, visualPoints;
        
        [Header("LevelValues")]
        public int startingPoints;
        public int endOfLevelCost;

        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;
        public GuidedTutorial guidedTutorialManager;

        [Header("Sound")] public World1LevelSoundController soundController;

        private static int lowValuePoints = 20;
        private static int gooseEggPoints = 50;
        private static int crystalShardPoints = 50;
        
        private DateTime lastVisualUpdate = DateTime.Now;
        private const float updateInterval = 0.01666667f;
        private const int speedFactor = 15; //Higher is slower

        private void Start() {
            totalPoints = startingPoints;
            visualPoints = totalPoints;
            UpdatePointText();

            endOfLevelCostText.text = endOfLevelCost.ToString();
        }

        //private void FixedUpdate() {
        //    totalPointsText.text = totalPoints.ToString();
        //}

        public void AddLowValuePoints() {
            IncrementPoints(lowValuePoints);
        }

        public void AddGooseEggPoints() {
            IncrementPoints(gooseEggPoints);
        }
        
        public void AddCrystalShardPoints() {
            IncrementPoints(crystalShardPoints);
        }

        public void BuyEndOfLevel() {
            if (totalPoints >= endOfLevelCost) {
                if ((int)slider.value == (int)slider.maxValue) {
                    LevelCompleted();
                } else {
                    DecrementPoints(endOfLevelCost);
                    slider.value += 1;
                }
            }
        }

        public void Update() {
            UpdateVisualPoints();
        }


        public void UpdateVisualPoints() {
            DateTime now = DateTime.Now;
            if ((now - lastVisualUpdate).TotalSeconds > updateInterval) {
                int difference = Math.Abs(visualPoints - totalPoints);
                int changeAmount = Math.Max(1, difference / speedFactor); 

                if (visualPoints < totalPoints) {
                    visualPoints = Math.Min(visualPoints + changeAmount, totalPoints);
                } else if (visualPoints > totalPoints) {
                    visualPoints = Math.Max(visualPoints - changeAmount, totalPoints);
                }

                lastVisualUpdate = now;
                UpdatePointText();
            }
        }

        void UpdatePointText() {
            if (totalPointsText != null) {
                totalPointsText.text = visualPoints.ToString();
            }
        }

        public void IncrementPoints(int amount) {
            totalPoints += amount;
        }

        public void DecrementPoints(int amount) {
            totalPoints -= amount;
        }
        
        //maybe find a different place for this logic
        private void LevelCompleted() {
            soundController.StopAudioEvent("Music");
            soundController.StopAudioEvent("Ambience");
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() => {
                guidedTutorialManager.enabled = false;
                StartCoroutine(LoadAsynchronously("NewCompanionScene"));
            })).SetUpdate(true);
        }

        IEnumerator LoadAsynchronously(string sceneIndex) {
            loadingScreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            yield return operation;
            loadingScreen.SetActive(false);
        }
    }
}
