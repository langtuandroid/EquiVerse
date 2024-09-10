using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LevelAchievement))]
public class LevelAchievementEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 0;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;
        float currentY = position.y;

        // Draw Achievement Reward
        SerializedProperty achievementReward = property.FindPropertyRelative("achievementReward");
        Rect achievementRewardRect = new Rect(position.x, currentY, position.width, lineHeight);
        EditorGUI.PropertyField(achievementRewardRect, achievementReward);
        currentY += lineHeight + padding;

        // Draw Achievement Type
        SerializedProperty achievementType = property.FindPropertyRelative("achievementType");
        Rect achievementTypeRect = new Rect(position.x, currentY, position.width, lineHeight);
        EditorGUI.PropertyField(achievementTypeRect, achievementType);
        currentY += lineHeight + padding;

        // Show specific fields based on Achievement Type
        AchievementType type = (AchievementType)achievementType.enumValueIndex;

        switch (type)
        {
            case AchievementType.TimeBased:
                SerializedProperty timeLimit = property.FindPropertyRelative("timeLimit");
                Rect timeLimitRect = new Rect(position.x, currentY, position.width, lineHeight);
                EditorGUI.PropertyField(timeLimitRect, timeLimit);
                currentY += lineHeight + padding;
                break;
            case AchievementType.AnimalDeaths:
                SerializedProperty maxAnimalDeaths = property.FindPropertyRelative("maxAnimalDeaths");
                Rect maxAnimalDeathsRect = new Rect(position.x, currentY, position.width, lineHeight);
                EditorGUI.PropertyField(maxAnimalDeathsRect, maxAnimalDeaths);
                currentY += lineHeight + padding;
                break;
            case AchievementType.AnimalsSpawned:
                SerializedProperty animalsSpawned = property.FindPropertyRelative("animalsSpawned");
                Rect animalsSpawnedRect = new Rect(position.x, currentY, position.width, lineHeight);
                EditorGUI.PropertyField(animalsSpawnedRect, animalsSpawned);
                currentY += lineHeight + padding;
                break;
            case AchievementType.LeafpointsCollected:
                SerializedProperty leafpointsCollected = property.FindPropertyRelative("leafpointsCollected");
                Rect leafpointsCollectedRect = new Rect(position.x, currentY, position.width, lineHeight);
                EditorGUI.PropertyField(leafpointsCollectedRect, leafpointsCollected);
                currentY += lineHeight + padding;
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;
        int lines = 5; // Base lines for common fields

        AchievementType type = (AchievementType)property.FindPropertyRelative("achievementType").enumValueIndex;
        switch (type)
        {
            case AchievementType.TimeBased:
            case AchievementType.AnimalDeaths:
            case AchievementType.AnimalsSpawned:
            case AchievementType.LeafpointsCollected:
                
                lines += 1;
                break;
        }

        return (lineHeight + padding) * lines;
    }
}

