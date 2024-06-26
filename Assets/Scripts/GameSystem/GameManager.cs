using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        
        public TextMeshProUGUI currentLevelText;

        private void Start()
        {
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
        }
        
        public static void LoadGameData()
        {
            WORLD_INDEX = PlayerPrefs.GetInt("WORLD_INDEX", 0);
            LEVEL_INDEX = PlayerPrefs.GetInt("LEVEL_INDEX", 0);
            companionsUnlockedIndex = PlayerPrefs.GetInt("companionsUnlockedIndex", 0);
            currentCompanionIndex = PlayerPrefs.GetInt("CurrentCompanionIndex", 0);
            firstTimePlaying = PlayerPrefs.GetInt("firstTimePlaying", 1) == 1;
        }
    }
}
