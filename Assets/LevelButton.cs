using System;
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

    public void UpdateButton(bool isCompleted)
    {
        levelButton.interactable = isCompleted;
        //UpdateAchievements();
    }

    // void UpdateAchievements()
    // {
    //     string levelKey = $"WORLD_{GameManager.WORLD_INDEX}_LEVEL_{levelIndex}";
    //     var levelAchievements = achievementManager.GetLevelAchievements();
    //
    //     for (int i = 0; i < achievementImages.Length; i++)
    //     {
    //         if (i < levelAchievements.Count)
    //         {
    //             var achievement = levelAchievements[i];
    //             switch (achievement.achievementState)
    //             {
    //                 case LevelAchievement.AchievementState.NotAchieved:
    //                     achievementImages[i].color = notAchievedColor;
    //                     break;
    //                 case LevelAchievement.AchievementState.NewlyAchieved:
    //                     achievementImages[i].color = newlyAchievedColor;
    //                     break;
    //                 case LevelAchievement.AchievementState.AlreadyAchieved:
    //                     achievementImages[i].color = alreadyAchievedColor;
    //                     break;
    //             }
    //         }
    //     }
    // }

    void OnButtonClicked()
    {
        // Load the level corresponding to levelIndex
        SceneManager.LoadScene($"Level {worldIndex}-{levelIndex}");
    }
}