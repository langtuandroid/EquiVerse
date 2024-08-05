using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Image[] achievementImages;
    public Button levelButton;

    public Color notAchievedColor;
    public Color newlyAchievedColor;
    public Color alreadyAchievedColor;

    private int levelIndex;
    private int worldIndex;
    
    void Start()
    {
        levelButton.onClick.AddListener(OnButtonClicked);
    }

    public void Initialize(int worldIndex, int levelIndex)
    {
        this.worldIndex = worldIndex;
        this.levelIndex = levelIndex;
        levelText.text = $"{worldIndex} - {levelIndex}";
        
        string levelKey = $"WORLD_{worldIndex}_LEVEL_{levelIndex}";
        bool isCompleted = AchievementManager.IsLevelPreviouslyCompleted(levelKey);
        levelButton.interactable = isCompleted;
        
        int achievementIndex = 0;
        for (int i = 0; i < (int)AchievementType._Count; i++)
        {
            string achievementKey = $"{levelKey}_{(AchievementType)i}";
            int stateValue = PlayerPrefs.GetInt(achievementKey, -999);
            if (stateValue != -999)
            {
                LevelAchievement.AchievementState state = (LevelAchievement.AchievementState)stateValue;
                switch (state)
                {
                    case LevelAchievement.AchievementState.NotAchieved:
                        achievementImages[achievementIndex].color = notAchievedColor;
                        break;
                    case LevelAchievement.AchievementState.NewlyAchieved:
                        achievementImages[achievementIndex].color = newlyAchievedColor;
                        break;
                    case LevelAchievement.AchievementState.AlreadyAchieved:
                        achievementImages[achievementIndex].color = alreadyAchievedColor;
                        break;
                    default:
                        throw new Exception($"Unknown state: {state} {stateValue}");
                        achievementImages[achievementIndex].color = Color.red;
                        break;
                }
                achievementIndex++;
            }
        }

    }

    void OnButtonClicked()
    {
        MainMenuSoundController soundController = FindObjectOfType<MainMenuSoundController>();
        soundController.FadeMainMenuVolume(1.0f, 1.1f);
        if ((worldIndex == 1 && levelIndex == 5) || worldIndex > 1)
        {
            SceneManager.LoadSceneAsync("CompanionSelectorScene", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync($"Level {worldIndex}-{levelIndex}", LoadSceneMode.Single);
        }
    }

}