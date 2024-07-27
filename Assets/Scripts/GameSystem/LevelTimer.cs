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
            levelKey = GetLevelKey();
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

        private string GetLevelKey()
        {
            GameManager gm = GetComponent<GameManager>();
            return $"Level_{gm.currentSceneWorldIndex}_{gm.currentSceneLevelIndex}_CompletionTime";
        }
    }
}


