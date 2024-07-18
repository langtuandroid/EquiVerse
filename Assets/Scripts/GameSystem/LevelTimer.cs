using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class LevelTimer : MonoBehaviour
    {
        private float startTime;
        private float endTime;
        private string levelKey;

        public void StartLevelTimer()
        {
            startTime = Time.time;
            levelKey = $"Level_{GameManager.WORLD_INDEX}_{GameManager.LEVEL_INDEX}_CompletionTime";
        }

        public void EndLevelTimer()
        {
            endTime = Time.time;
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

        public void DisplayCompletionTime(TextMeshProUGUI textComponent)
        {
            float completionTime = LoadCompletionTime();
            TimeSpan timeSpan = TimeSpan.FromSeconds(completionTime);
            textComponent.text = $"Level {GameManager.WORLD_INDEX} - {GameManager.LEVEL_INDEX} Completion Time: {timeSpan:mm\\:ss}";
        }
    }
}

