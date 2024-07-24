using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
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

    public bool isAchieved;
    public Image backGroundImage;
    public AchievementState achievementState;

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
        Debug.Log($"Saved achievement: {achievementKey} with state {achievementState}");
    }

    public void LoadAchievement(string levelKey)
    {
        string achievementKey = $"{levelKey}_{achievementType}";
        Debug.Log($"loadachiements: {achievementKey}");
        int stateValue = PlayerPrefs.GetInt(achievementKey, (int)AchievementState.NotAchieved);
        achievementState = (AchievementState)stateValue;
        Debug.Log($"Loaded achievement: {achievementKey} with state {achievementState}");
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
        achievementChecker.CheckAchievements();
        foreach (var achievement in levelAchievements)
        {
            if (achievement.isAchieved && achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
            }
            else if (achievement.isAchieved && achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved)
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
        popUpUIBehaviour.DisplayAchievements(levelAchievements);
        SaveAchievements();
    }
    
    public void SaveAchievements()
    {
        string currentLevelKey = GetCurrentLevelKey();
        foreach (var achievement in levelAchievements)
        {
            achievement.SaveAchievement(currentLevelKey);
        }
    }

    public void LoadAchievements()
    {
        string currentLevelKey = GetCurrentLevelKey();
        foreach (var achievement in levelAchievements)
        {
            achievement.LoadAchievement(currentLevelKey);
        }
    }

    private void InitializeAchievementTypeData()
    {
        achievementTypeData = new Dictionary<AchievementType, (Sprite, string)>
        {
            { AchievementType.FinishedLevel, (finishedLevelImage, "Completed the level") },
            { AchievementType.TimeBased, (timeBasedImage, "Complete the level within {value} minutes") },
            { AchievementType.AnimalDeaths, (animalDeathsImage, "Keep the animal deaths below {value}") },
            { AchievementType.AnimalsSpawned, (AnimalsSpawnedImage, "Spawn at least {value} animals") },
            { AchievementType.LeafpointsCollected, (leafPointsImage, "Collect at least {value} Leaf points") }
        };
    }
    
    private string GetCurrentLevelKey()
    {
        return $"WORLD_{GameManager.WORLD_INDEX}_LEVEL_{GameManager.LEVEL_INDEX}";
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
