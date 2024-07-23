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
            if (achievement.achievementType == AchievementType.FinishedLevel && !achievement.isAchieved)
            {
                if (GameManager.levelCompletionStatus.TryGetValue(currentLevelKey, out bool isCompleted) && isCompleted)
                {
                    achievement.isAchieved = true;
                    Debug.Log($"Achievement achieved: {achievement.achievementDescription}");
                }
                else
                {
                    achievement.isAchieved = false;
                    Debug.Log($"Achievement not achieved: {achievement.achievementDescription}");
                }
            }
        }
    }

    private void CheckTimeBasedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.TimeBased && !achievement.isAchieved)
            {
                float completionTime = levelTimer.LoadCompletionTime();
                if (completionTime > 0 && completionTime <= (achievement.timeLimit * 60))
                {
                    print(achievement.timeLimit);
                    print(levelTimer.LoadCompletionTime());
                    achievement.isAchieved = true;
                    Debug.Log($"Achievement achieved: {achievement.achievementDescription}");
                }
                else
                {
                    achievement.isAchieved = false;
                    Debug.Log($"Achievement not achieved: {achievement.achievementDescription}");
                }
            }
        }
    }

    private void CheckAnimalDeathsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.AnimalDeaths && !achievement.isAchieved)
            {
                //TODO: Change to actual data
                if (GameManager.animalDeaths <= achievement.maxAnimalDeaths)
                {
                    achievement.isAchieved = true;
                    Debug.Log($"Achievement achieved: {achievement.achievementDescription}");
                }
                else
                {
                    achievement.isAchieved = false;
                    Debug.Log($"Achievement not achieved: {achievement.achievementDescription}");
                }
            }
        }
    }

    private void CheckLeafPointsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.LeafpointsCollected && !achievement.isAchieved)
            {
                if (achievement.leafpointsCollected >= GameManager.totalLeafPointsCollected)
                {
                    achievement.isAchieved = true;
                    Debug.Log($"Achievement achieved: {achievement.achievementDescription}");
                }
                else
                {
                    achievement.isAchieved = false;
                    Debug.Log($"Achievement not achieved: {achievement.achievementDescription}");
                }
            }
        }
    }
    private void CheckAnimalsSpawnedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.AnimalsSpawned && !achievement.isAchieved)
            {
                if (GameManager.animalsSpawned >= achievement.animalsSpawned)
                {
                    achievement.isAchieved = true;
                    Debug.Log($"Achievement achieved: {achievement.achievementDescription}");
                }
                else
                {
                    achievement.isAchieved = false;
                    Debug.Log($"Achievement not achieved: {achievement.achievementDescription}");
                }
            }
        }
    }
}
