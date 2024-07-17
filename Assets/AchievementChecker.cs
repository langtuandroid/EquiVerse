using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementChecker : MonoBehaviour
{
    public AchievementManager achievementManager;

    public void CheckAchievements()
    {
        CheckTimeBasedAchievement();
        CheckAnimalDeathsAchievement();
        CheckLeafPointsAchievement();
    }
    
    
    private void CheckTimeBasedAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.TimeBased && !achievement.isAchieved)
            {
                //TODO: Change to actual data
                if (achievement.timeLimit > 0)
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

    private void CheckAnimalDeathsAchievement()
    {
        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            if (achievement.achievementType == AchievementType.AnimalDeaths && !achievement.isAchieved)
            {
                //TODO: Change to actual data
                if (2 <= achievement.maxAnimalDeaths)
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
            if (achievement.achievementType == AchievementType.LeafPoints && !achievement.isAchieved)
            {
                //TODO: Change to actual data
                if (achievement.leafPointsCollected > 5)
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
