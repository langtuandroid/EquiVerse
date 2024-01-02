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
        public float lerpDuration = 0.5f;
        public Slider slider;
        public static int totalPoints, visualPoints;
        public int startingPoints;
        public int lowValuePoints;
        public int gooseEggPoints;
        public int endOfLevelCost;

        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;
        public GuidedTutorial guidedTutorialManager;

        private DateTime lastVisualUpdate = DateTime.Now;
        private const float updateInterval = 0.01666667f;
        private const int speedFactor = 15; //Higher is slower

        private void Start() {
            totalPoints = startingPoints;
            visualPoints = totalPoints;
            UpdatePointText();
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


        //IEnumerator LerpPoints(int startPoints, int targetPoints)
        //{
        //    float elapsedTime = 0f;
        //
        //    while (elapsedTime < lerpDuration)
        //    {
        //        totalPoints = Mathf.RoundToInt(Mathf.Lerp(startPoints, targetPoints, elapsedTime / lerpDuration));
        //        UpdatePointText();
        //        elapsedTime += Time.deltaTime;
        //        yield return null;
        //    }
        //
        //    totalPoints = targetPoints;
        //    UpdatePointText();
        //    lerpCoroutine = null; // Set coroutine to null when finished
        //}

        void UpdatePointText() {
            if (totalPointsText != null) {
                totalPointsText.text = visualPoints.ToString();
            }
        }

        public void IncrementPoints(int amount) {
            totalPoints += amount;
            //if (lerpCoroutine != null)
            //{
            //    StopCoroutine(lerpCoroutine); // Stop the current coroutine if it's running
            //}
            //
            //int newPoints = totalPoints + amount;
            //lerpCoroutine = StartCoroutine(LerpPoints(totalPoints, newPoints));
        }

        public void DecrementPoints(int amount) {
            totalPoints -= amount;

            //if (lerpCoroutine != null)
            //{
            //    StopCoroutine(lerpCoroutine); // Stop the current coroutine if it's running
            //}
            //
            //int newPoints = totalPoints - amount;
            //lerpCoroutine = StartCoroutine(LerpPoints(totalPoints, newPoints));
        }

        private void LevelCompleted() {
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
