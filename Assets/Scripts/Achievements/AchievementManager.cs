using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum AchievementType
{
    FinishedLevel,
    TimeBased,
    AnimalDeaths,
    AnimalsSpawned,
    LeafpointsCollected,
    _Count
}

[System.Serializable]
public class LevelAchievement
{
    public int achievementReward;
    public AchievementType achievementType;
    public float timeLimit;
    public int maxAnimalDeaths;
    public int animalsSpawned;
    public int leafpointsCollected;

    [HideInInspector]
    public Sprite achievementImage;
    [HideInInspector]
    public string achievementDescription;

    public Image backGroundImage;
    public AchievementState achievementState = AchievementState.NotAchieved;

    public enum AchievementState
    {
        NotAchieved,
        NewlyAchieved,
        AlreadyAchieved
    }

    public void SaveAchievement(string levelKey)
    {
        string achievementKey = $"{levelKey}_{achievementType}";
        int stateValue = (int)achievementState;
        PlayerPrefs.SetInt(achievementKey, stateValue);
    }

    public void LoadAchievement(string levelKey)
    {
        string achievementKey = $"{levelKey}_{achievementType}";
        int stateValue = PlayerPrefs.GetInt(achievementKey, (int)AchievementState.NotAchieved);
        achievementState = (AchievementState)stateValue;
    }
}

public class AchievementManager : MonoBehaviour
{
    public AchievementChecker achievementChecker;
    public List<LevelAchievement> levelAchievements;
    public PopUpLevelCompletionUIBehaviour popUpUIBehaviour;

    private Dictionary<AchievementType, (Sprite, string)> achievementTypeData;

    public Sprite finishedLevelImage;
    public Sprite timeBasedImage;
    public Sprite animalDeathsImage;
    public Sprite leafPointsImage;
    public Sprite AnimalsSpawnedImage;
    
    

    private void Start()
    {
        InitializeAchievementTypeData();

        foreach (var achievement in levelAchievements)
        {
            if (achievementTypeData.TryGetValue(achievement.achievementType, out var data))
            {
                achievement.achievementImage = data.Item1;
                achievement.achievementDescription = GenerateDescription(data.Item2, achievement);
            }
        }
        
        LoadAchievements();
    }

    public void ActivateAchievements()
    {
        foreach (var achievement in levelAchievements)
        {
            if (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved)
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
        achievementChecker.CheckAchievements();
        SaveAchievements();
        popUpUIBehaviour.DisplayAchievements(levelAchievements);
    }
    
    public void SaveAchievements()
    {
        GameManager gm = GetComponent<GameManager>();
        foreach (var achievement in levelAchievements)
        {
            achievement.SaveAchievement(gm.GetCurrentLevelKey());
        }
    }

    public void LoadAchievements()
    {
        GameManager gm = GetComponent<GameManager>();
        foreach (var achievement in levelAchievements)
        {
            achievement.LoadAchievement(gm.GetCurrentLevelKey());
        }
    }
    
    public static bool IsLevelPreviouslyCompleted(string levelKey)
    {
        string achievementKey = $"{levelKey}_{AchievementType.FinishedLevel}";
        int stateValue = PlayerPrefs.GetInt(achievementKey, (int)LevelAchievement.AchievementState.NotAchieved);
        return stateValue != (int)LevelAchievement.AchievementState.NotAchieved;
    }

    private void InitializeAchievementTypeData()
    {
        achievementTypeData = new Dictionary<AchievementType, (Sprite, string)>
        {
            { AchievementType.FinishedLevel, (finishedLevelImage, "Completed the level") },
            { AchievementType.TimeBased, (timeBasedImage, "Complete the level within {value} minutes") },
            { AchievementType.AnimalDeaths, (animalDeathsImage, "Keep the animal deaths below {value}") },
            { AchievementType.AnimalsSpawned, (AnimalsSpawnedImage, "Spawn a maximal of {value} animals") },
            { AchievementType.LeafpointsCollected, (leafPointsImage, "Collect at least {value} Leaf points") }
        };
    }

    private string GenerateDescription(string template, LevelAchievement achievement)
    {
        switch (achievement.achievementType)
        {
            case AchievementType.FinishedLevel:
                return template;
            case AchievementType.TimeBased:
                return template.Replace("{value}", achievement.timeLimit.ToString());
            case AchievementType.AnimalDeaths:
                return template.Replace("{value}", achievement.maxAnimalDeaths.ToString());
            case AchievementType.AnimalsSpawned:
                return template.Replace("{value}", achievement.animalsSpawned.ToString());
            case AchievementType.LeafpointsCollected:
                return template.Replace("{value}", achievement.leafpointsCollected.ToString());
            default:
                return template;
        }
    }

    public List<LevelAchievement> GetLevelAchievements()
    {
        return levelAchievements;
    }
}
