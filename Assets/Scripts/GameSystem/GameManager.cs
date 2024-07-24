using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static int WORLD_INDEX;
        public static int LEVEL_INDEX;
        public static int companionsUnlockedIndex;
        public static int currentCompanionIndex;
        public static int currentVideoStepIndex;
        public bool level1;
        public bool level2;
        public bool level3;
        public bool level4;
        public bool level5;
        public static bool firstTimePlaying = true;
        public static string discordUrl = "https://discord.gg/hay2fMBggT";
        public static int animalDeaths;
        public static int totalLeafPointsCollected;
        public static int animalsSpawned;
        public static int totalLevels = 5;
        public static int totalWorlds = 4;
        
        public static Dictionary<string, bool> levelCompletionStatus = new Dictionary<string, bool>();
        public TextMeshProUGUI currentLevelText;

        private void Start()
        {
            animalDeaths = 0;
            totalLeafPointsCollected = 0;
            animalsSpawned = 0;
            
            string currentLevelKey = GetCurrentLevelKey();
            if (!levelCompletionStatus.ContainsKey(currentLevelKey))
            {
                levelCompletionStatus[currentLevelKey] = false;
            }
            
            currentLevelText.text = "Level " + WORLD_INDEX + " - " + LEVEL_INDEX;
            SaveGameData();
        }

        public static void SaveGameData()
        {
            PlayerPrefs.SetInt("WORLD_INDEX", WORLD_INDEX);
            PlayerPrefs.SetInt("LEVEL_INDEX", LEVEL_INDEX);
            PlayerPrefs.SetInt("companionsUnlockedIndex", companionsUnlockedIndex);
            PlayerPrefs.SetInt("CurrentCompanionIndex", currentCompanionIndex);
            PlayerPrefs.SetInt("firstTimePlaying", firstTimePlaying ? 1 : 0);
            PlayerPrefs.Save();
            
            foreach (var kvp in levelCompletionStatus)
            {
                PlayerPrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            }
        }

        public static void LoadGameData()
        {
            WORLD_INDEX = PlayerPrefs.GetInt("WORLD_INDEX", 0);
            LEVEL_INDEX = PlayerPrefs.GetInt("LEVEL_INDEX", 0);
            companionsUnlockedIndex = PlayerPrefs.GetInt("companionsUnlockedIndex", 0);
            currentCompanionIndex = PlayerPrefs.GetInt("CurrentCompanionIndex", 0);
            firstTimePlaying = PlayerPrefs.GetInt("firstTimePlaying", 1) == 1;
            
            levelCompletionStatus.Clear();
            for (int world = 0; world <= WORLD_INDEX; world++)
            {
                for (int level = 0; level <= LEVEL_INDEX; level++)
                {
                    string key = $"WORLD_{world}_LEVEL_{level}";
                    levelCompletionStatus[key] = PlayerPrefs.GetInt(key, 0) == 1;
                }
            }
        }
        
        private string GetCurrentLevelKey()
        {
            return $"WORLD_{WORLD_INDEX}_LEVEL_{LEVEL_INDEX}";
        }
    }
}
