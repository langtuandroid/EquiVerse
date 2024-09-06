using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class LevelTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        
        private float startTime;
        private float endTime;
        private string levelKey;
        private bool isTiming;

        private void Update()
        {
            if (isTiming)
            {
                UpdateTimerDisplay();
            }
        }

        public void StartLevelTimer()
        {
            startTime = Time.time;
            isTiming = true;
            levelKey = GetLevelKey();
        }

        public void EndLevelTimer()
        {
            endTime = Time.time;
            isTiming = false;
            float completionTime = endTime - startTime;
            SaveCompletionTime(completionTime);
        }

        private void SaveCompletionTime(float completionTime)
        {
            PlayerPrefs.SetFloat(levelKey, completionTime);
            PlayerPrefs.Save();
        }

        public float LoadCompletionTime()
        {
            return PlayerPrefs.GetFloat(levelKey, 0f); // Default to 0 if no time is saved
        }

        private string GetLevelKey()
        {
            GameManager gm = GetComponent<GameManager>();
            return $"Level_{gm.currentSceneWorldIndex}_{gm.currentSceneLevelIndex}_CompletionTime";
        }

        private void UpdateTimerDisplay()
        {
            float currentTime = Time.time - startTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}


