using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelStat
{
    public string statName;
    public string statValue;
}

public enum AchievementType
{
    FinishedLevel,
    TimeBased,
    AnimalDeaths,
    LeafPoints
}

[System.Serializable]
public class LevelAchievement
{
    public int achievementReward;
    public AchievementType achievementType;
    
    public float timeLimit;
    public int maxAnimalDeaths;
    public int leafPointsCollected;

    [HideInInspector]
    public Sprite achievementImage;
    [HideInInspector]
    public string achievementDescription;

    public Image backGroundImage;
    public bool isAchieved;
}

public class AchievementManager : MonoBehaviour
{
    public AchievementChecker achievementChecker;
    public List<LevelStat> levelStats; 
    public List<LevelAchievement> levelAchievements;
    public PopUpLevelCompletionUIBehaviour popUpUIBehaviour;

    private Dictionary<AchievementType, (Sprite, string)> achievementTypeData;

    public Sprite finishedLevelImage;
    public Sprite timeBasedImage;
    public Sprite animalDeathsImage;
    public Sprite leafPointsImage;

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
        
        InitializeLevelStats();
    }

    public void ActivateAchievements()
    {
        achievementChecker.CheckAchievements();
        popUpUIBehaviour.DisplayAchievements(levelAchievements, levelStats);
    }

    private void InitializeAchievementTypeData()
    {
        achievementTypeData = new Dictionary<AchievementType, (Sprite, string)>
        {
            { AchievementType.FinishedLevel, (finishedLevelImage, "Completed the level") },
            { AchievementType.TimeBased, (timeBasedImage, "Complete the level within {value} minutes") },
            { AchievementType.AnimalDeaths, (animalDeathsImage, "Keep the animal deaths below {value}") },
            { AchievementType.LeafPoints, (leafPointsImage, "Collect {value} leaf points") }
        };
    }
    
    private void InitializeLevelStats()
    {
        levelStats = new List<LevelStat>
        {
            new LevelStat { statName = "Completion Time", statValue = "00:00" },
            new LevelStat { statName = "Enemies Defeated", statValue = "0" },
            new LevelStat { statName = "Points Collected", statValue = "0" }
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
            case AchievementType.LeafPoints:
                return template.Replace("{value}", achievement.leafPointsCollected.ToString());
            default:
                return template;
        }
    }

    public List<LevelAchievement> GetLevelAchievements()
    {
        return levelAchievements;
    }
}
