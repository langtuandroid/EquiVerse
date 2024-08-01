using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static int currentVideoStepIndex;
        public bool level1;
        public bool level2;
        public bool level3;
        public bool level4;
        public bool level5;
        public int currentSceneWorldIndex;
        public int currentSceneLevelIndex;
        
        public int nextSceneWorldIndex;
        public int nextSceneLevelIndex;
        public static bool firstTimePlaying = true;
        public static string discordUrl = "https://discord.gg/hay2fMBggT";
        public static int animalDeaths;
        public static int totalLeafPointsCollected;
        public static int animalsSpawned;
        public static int totalLevels = 5;
        public static int totalWorlds = 4;
        
        public TextMeshProUGUI currentLevelText;

        private void Start()
        {
            animalDeaths = 0;
            totalLeafPointsCollected = 0;
            animalsSpawned = 0;
            
            
            string currentLevelKey = GetCurrentLevelKey();
            
            currentLevelText.text = "Level " + currentSceneWorldIndex + " - " + currentSceneLevelIndex;
            SaveGameData();
            StoreNextLevelInPlayerPrefs();
        }
        
        public static void SaveGameData()
        {
            PlayerPrefs.SetInt("firstTimePlaying", firstTimePlaying ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void LoadGameData()
        {
            firstTimePlaying = PlayerPrefs.GetInt("firstTimePlaying", 1) == 1;
        }

        private void StoreNextLevelInPlayerPrefs()
        {
            PlayerPrefs.SetInt("nextSceneLevelIndex", nextSceneLevelIndex);
            PlayerPrefs.SetInt("nextSceneWorldIndex", nextSceneWorldIndex);
        }

        public static (int, int) GetNextLevelFromPlayerPrefs()
        {
            int level = PlayerPrefs.GetInt("nextSceneLevelIndex", -1);
            int world = PlayerPrefs.GetInt("nextSceneWorldIndex", -1);
            return (level, world);
        }

        public static string GetNextLevelKeyFromPlayerPrefs()
        {
            (int level, int world) = GameManager.GetNextLevelFromPlayerPrefs();
            return $"WORLD_{world}_LEVEL_{level}";
        } 
        
        public string GetCurrentLevelKey()
        {
            return $"WORLD_{currentSceneWorldIndex}_LEVEL_{currentSceneLevelIndex}";
        }
        
        
        private static List<ValueTuple<int, int>> levels = new List<(int, int)>()
        {
            new ValueTuple<int, int>(1, 1),
            new ValueTuple<int, int>(1, 2),
            new ValueTuple<int, int>(1, 3),
            new ValueTuple<int, int>(1, 4),
            new ValueTuple<int, int>(1, 5),
            new ValueTuple<int, int>(2, 1),
            new ValueTuple<int, int>(2, 2),
            new ValueTuple<int, int>(2, 3),
            new ValueTuple<int, int>(2, 4),
            new ValueTuple<int, int>(2, 5),
            new ValueTuple<int, int>(3, 1),
            new ValueTuple<int, int>(3, 2),
            new ValueTuple<int, int>(3, 3),
            new ValueTuple<int, int>(3, 4),
            new ValueTuple<int, int>(3, 5),
            new ValueTuple<int, int>(4, 1),
            new ValueTuple<int, int>(4, 2),
            new ValueTuple<int, int>(4, 3),
            new ValueTuple<int, int>(4, 4),
            new ValueTuple<int, int>(4, 5),
        };
        
        public static (int, int, int) FindFirstUncompletedLevel()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                (int world, int level) = levels[i];
                string levelKey = $"WORLD_{world}_LEVEL_{level}";
                if (!AchievementManager.IsLevelPreviouslyCompleted(levelKey)) return (i, levels[i].Item1, levels[i].Item2);
            }

            return (-1, -1, -1);
        }

        public int GetCurrentLevelIndex() //The current level index is also the companion index + 1
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].Item1 == currentSceneWorldIndex && levels[i].Item2 == currentSceneLevelIndex) return i;
            }

            return -1;
        }

        public int GetCurrentLevelCompanionIndex()
        {
            return GetCurrentLevelIndex() - 1;
        }
    }
}
