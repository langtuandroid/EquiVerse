using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AchievementType
{
    FinishedLevel,
    TimeBased,
    AnimalDeaths,
    AnimalsSpawned,
    LeafpointsCollected
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
    public bool isAchieved;
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
    }

    public void ActivateAchievements()
    {
        achievementChecker.CheckAchievements();
        popUpUIBehaviour.DisplayAchievements(levelAchievements);
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
