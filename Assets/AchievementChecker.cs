using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class AchievementChecker : MonoBehaviour
{
    public LevelTimer levelTimer;
    public AchievementManager achievementManager;

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
        string currentLevelKey = $"WORLD_{GameManager.WORLD_INDEX}_LEVEL_{GameManager.LEVEL_INDEX}";

        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.FinishedLevel && (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved))
            {
                if (GameManager.levelCompletionStatus.TryGetValue(currentLevelKey, out bool isCompleted) && isCompleted)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
                else
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NotAchieved;
                }
            }else if (achievement.achievementType == AchievementType.FinishedLevel && (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved))
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
    }

    private void CheckTimeBasedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.TimeBased && (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved))
            {
                float completionTime = levelTimer.LoadCompletionTime();
                if (completionTime > 0 && completionTime <= (achievement.timeLimit * 60))
                {
                    print(achievement.timeLimit);
                    print(levelTimer.LoadCompletionTime());
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
                else
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NotAchieved;
                }
            }else if (achievement.achievementType == AchievementType.TimeBased && (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved))
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
    }

    private void CheckAnimalDeathsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.AnimalDeaths && (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved))
            {
                //TODO: Change to actual data
                if (GameManager.animalDeaths <= achievement.maxAnimalDeaths)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
                else
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NotAchieved;
                }
            }else if (achievement.achievementType == AchievementType.AnimalDeaths && (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved))
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
    }

    private void CheckLeafPointsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.LeafpointsCollected && (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved))
            {
                if (achievement.leafpointsCollected >= GameManager.totalLeafPointsCollected)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
                else
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NotAchieved;
                }
            }else if (achievement.achievementType == AchievementType.LeafpointsCollected && (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved))
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
    }
    private void CheckAnimalsSpawnedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.AnimalsSpawned && (achievement.achievementState == LevelAchievement.AchievementState.NotAchieved))
            {
                if (GameManager.animalsSpawned >= achievement.animalsSpawned)
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NewlyAchieved;
                }
                else
                {
                    achievement.achievementState = LevelAchievement.AchievementState.NotAchieved;
                }
            }else if (achievement.achievementType == AchievementType.AnimalsSpawned && (achievement.achievementState == LevelAchievement.AchievementState.NewlyAchieved))
            {
                achievement.achievementState = LevelAchievement.AchievementState.AlreadyAchieved;
            }
        }
    }
}
