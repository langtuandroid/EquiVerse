using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class AchievementChecker : MonoBehaviour
{
    public LevelTimer levelTimer;
    public AchievementManager achievementManager;

    public static bool firstTimeCompletion;

    private void Start()
    {
        firstTimeCompletion = false;
    }

    public void CheckAchievements()
    {
        CheckFinishedLevelAchievement();
        CheckTimeBasedAchievement();
        CheckAnimalDeathsAchievement();
        CheckLeafPointsAchievement();
        CheckAnimalsSpawnedAchievement();
    }

    private void CheckFinishedLevelAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType != AchievementType.FinishedLevel) continue;
            if (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                firstTimeCompletion = true;

            }
        }
    }

    private void CheckTimeBasedAchievement()
    {
        var fiets = achievementManager.GetLevelAchievements();
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType != AchievementType.TimeBased) continue;
            if (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                float completionTime = levelTimer.LoadCompletionTime();
                if (completionTime > 0 && completionTime <= achievement.timeLimit * 60)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
            }
        }
    }

    private void CheckAnimalDeathsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType != AchievementType.AnimalDeaths) continue;
            if (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                //TODO: Change to actual data
                if (GameManager.animalDeaths <= achievement.maxAnimalDeaths)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
            }
        }
    }

    private void CheckLeafPointsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType != AchievementType.LeafpointsCollected) continue;
            if (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                if (achievement.leafpointsCollected >= GameManager.totalLeafPointsCollected)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
            }
        }
    }
    private void CheckAnimalsSpawnedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType != AchievementType.AnimalsSpawned) continue;
            if (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved)
            {
                if (GameManager.animalsSpawned >= achievement.animalsSpawned)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
            }
        }
    }
}
