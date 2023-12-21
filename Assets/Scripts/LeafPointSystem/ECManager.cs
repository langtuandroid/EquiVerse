using System.Collections;
using DG.Tweening;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class ECManager : MonoBehaviour
    {
        public TextMeshProUGUI totalPointsText;
        public TextMeshProUGUI endOfLevelText;
        public Slider slider;

        [HideInInspector]
        public static float totalPoints;
        public float startingPoints = 10f;
        public float lowValuePoints = 10f;
        public float endOfLevelCost = 100f;
    
        [Header("SceneTransition")]
        public Image transitionOverlay;
        public GameObject loadingScreen;
        public GuidedTutorial guidedTutorialManager;
    
        private void Start()
        {
            totalPoints = 0;
            totalPoints += startingPoints;
        }

        private void FixedUpdate()
        {
            totalPointsText.text = totalPoints.ToString();
        }

        public void AddLowValuePoints()
        {
            totalPoints += lowValuePoints;
        }

        public void BuyEndOfLevel()
        {
        
            if (totalPoints >= endOfLevelCost)
            {
                if ((int)slider.value == (int)slider.maxValue)
                {
                    LevelCompleted();
                }
                else
                {
                    totalPoints -= endOfLevelCost;
                    slider.value += 1; 
                }
            }
        }

        private void LevelCompleted()
        {
            transitionOverlay.DOFade(1f, 1.2f).SetEase(Ease.InCubic).OnComplete((() =>
            {
                guidedTutorialManager.enabled = false;
                StartCoroutine(LoadAsynchronously("NewCompanionScene"));
            })).SetUpdate(true);
        }
    
        IEnumerator LoadAsynchronously(string sceneIndex)
        {
            loadingScreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            yield return operation;
            loadingScreen.SetActive(false);
        }
    }
}
