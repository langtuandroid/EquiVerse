using System;
using System.Collections;
using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers {
    public class LeafPointManager : MonoBehaviour
    {
        public Image totalPointsBackground;
        private Color originalColor;
        public TextMeshProUGUI totalPointsText;
        public TextMeshProUGUI endOfLevelCostText;
        public static int totalPoints, visualPoints;
        public GameObject[] locks;
        private int lockIndex = 0;
        public Button endOfLevelButton;
        private Tween lockTween;
        
        [Header("LevelValues")]
        public int startingPoints;
        public int endOfLevelCost;

        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;

        [Header("Sound")] 
        public World1LevelSoundController soundController;

        private static int lowValuePoints = 15;
        private static int highValuePoints = 35;
        private static int gooseEggPoints = 200;
        private static int crystalShardPoints = 200;
        
        private DateTime lastVisualUpdate = DateTime.Now;
        private const float updateInterval = 0.01666667f;
        private const int speedFactor = 15; //Higher is slower

        private bool finishLevelStepCompleted = true;

        private void Start()
        {
            originalColor = totalPointsBackground.color;
            
            totalPoints = startingPoints;
            visualPoints = totalPoints;
            UpdatePointText();

            endOfLevelCostText.text = endOfLevelCost.ToString();
        }

        private void FixedUpdate()
        {
            if (totalPoints >= endOfLevelCost && !finishLevelStepCompleted)
            {
                TutorialManager.GoToNextStep();
                finishLevelStepCompleted = true;
            }
        }
        
        public void Update() {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
            {
                totalPoints += 100;
            }
            UpdateVisualPoints();
        }

        public void AddLowValuePoints() {
            IncrementPoints(lowValuePoints);
        }
        
        public void AddHighValuePoints() {
            IncrementPoints(highValuePoints);
        }

        public void AddGooseEggPoints() {
            IncrementPoints(gooseEggPoints);
        }
        
        public void AddCrystalShardPoints() {
            IncrementPoints(crystalShardPoints);
        }

        public void BuyEndOfLevel() {
            if (totalPoints >= endOfLevelCost) {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buy");
                DecrementPoints(endOfLevelCost);
                RemoveLock();
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CantBuy");
                FlickerTotalPointsElement();
            }
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
        
        private void LevelCompleted()
        {
            endOfLevelButton.interactable = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/CompleteLevel");
            
            soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 1.2f);
            soundController.FadeAudioParameter("Ambience", "World1LevelAmbienceVolume", 0f, 1.2f);
            
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() => {
                soundController.StopAudioEvent("Music");
                soundController.StopAudioEvent("Ambience");
                StartCoroutine(LoadAsynchronously("NewCompanionScene"));
            })).SetUpdate(true);
        }

        public void FinishLevelStep(bool _finishLevelStepCompleted)
        {
            finishLevelStepCompleted = _finishLevelStepCompleted;
        }

        IEnumerator LoadAsynchronously(string sceneIndex) {
            loadingScreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            yield return operation;
            loadingScreen.SetActive(false);
        }

        public void FlickerTotalPointsElement()
        {
            totalPointsBackground.DOColor(new Color(1f, 0.3f, 0.35f, 0.8f), 0.25f)
                .SetLoops(5, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    totalPointsBackground.color = originalColor;
                });
        }

        private void RemoveLock()
        {
            lockTween.Complete();
            if (lockIndex < locks.Length)
            {
                print(locks.Length);
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Unlock");
                lockTween = locks[lockIndex].transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    locks[lockIndex].SetActive(false);
                    if (lockIndex == locks.Length - 1)
                    {
                        LevelCompleted();
                    }
                    else
                    {
                        lockIndex++; 
                    }
                });
            }
        }



    }
}
