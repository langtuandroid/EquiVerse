using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PermanentUpgradeManager))]
public class PermanentUpgradeManagerEditor : Editor
{
    private bool showUpgradeList = true;

    public override void OnInspectorGUI()
    {
        PermanentUpgradeManager manager = (PermanentUpgradeManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Permanent Upgrades Manager", EditorStyles.boldLabel);

        // Toggle for the entire upgrade list
        showUpgradeList = EditorGUILayout.Foldout(showUpgradeList, "Available Upgrades", true);

        if (showUpgradeList)
        {
            EditorGUILayout.BeginVertical("box");

            if (manager.availableUpgrades == null)
            {
                manager.availableUpgrades = new List<PermanentUpgrade>();
            }

            if (manager.availableUpgrades.Count == 0)
            {
                EditorGUILayout.LabelField("No upgrades available.", EditorStyles.miniBoldLabel);
            }
            else
            {
                for (int i = 0; i < manager.availableUpgrades.Count; i++)
                {
                    var upgrade = manager.availableUpgrades[i];

                    EditorGUILayout.BeginVertical("box");

                    // Upgrade Name and Description
                    upgrade.upgradeName = EditorGUILayout.TextField("Upgrade Name", upgrade.upgradeName);
                    upgrade.upgradeDescription = EditorGUILayout.TextField("Upgrade Description", upgrade.upgradeDescription);

                    // First, select the category
                    upgrade.upgradeCategory = (PermanentUpgradeCategory)EditorGUILayout.EnumPopup("Upgrade Category", upgrade.upgradeCategory);

                    // Then, based on the selected category, display the relevant upgrade types
                    PermanentUpgradeType[] filteredTypes = GetUpgradeTypesByCategory(upgrade.upgradeCategory);
                    int selectedIndex = ArrayUtility.IndexOf(filteredTypes, upgrade.upgradeType);
                    if (selectedIndex == -1) selectedIndex = 0;
                    upgrade.upgradeType = filteredTypes[EditorGUILayout.Popup("Upgrade Type", selectedIndex, GetUpgradeTypeNames(filteredTypes))];

                    // Upgrade Effect
                    upgrade.effectValue = EditorGUILayout.FloatField("Effect Value", upgrade.effectValue);

                    // Upgrade Cost
                    upgrade.upgradeCost = EditorGUILayout.IntField("Upgrade Cost", upgrade.upgradeCost);

                    // Upgrade Image
                    upgrade.upgradeImage = (Sprite)EditorGUILayout.ObjectField("Upgrade Image", upgrade.upgradeImage, typeof(Sprite), false);

                    // Required World and Level
                    upgrade.requiredWorld = EditorGUILayout.IntField("Required World", upgrade.requiredWorld);
                    upgrade.requiredLevel = EditorGUILayout.IntField("Required Level", upgrade.requiredLevel);

                    // Remove button
                    if (GUILayout.Button("Remove Upgrade"))
                    {
                        manager.availableUpgrades.RemoveAt(i);
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            }

            // Button to add a new upgrade
            if (GUILayout.Button("Add New Upgrade"))
            {
                manager.availableUpgrades.Add(new PermanentUpgrade());
            }

            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }
    }

    private PermanentUpgradeType[] GetUpgradeTypesByCategory(PermanentUpgradeCategory category)
    {
        List<PermanentUpgradeType> validTypes = new List<PermanentUpgradeType>();

        switch (category)
        {
            case PermanentUpgradeCategory.companionUpgrade:
                validTypes.Add(PermanentUpgradeType.increaseEggValueUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseEggSpawnFrequencyUpgrade);
                validTypes.Add(PermanentUpgradeType.increasePabloMoveSpeedUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseTobyThrowRateUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseTobyFoodQualityUpgrade);
                break;

            case PermanentUpgradeCategory.animalUpgrade:
                validTypes.Add(PermanentUpgradeType.increaseRabbitDeathThresholdUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseRabbitMoveSpeedUpgrade);
                validTypes.Add(PermanentUpgradeType.decreaseFoxHungerRate);
                break;

            case PermanentUpgradeCategory.combatUpgrade:
                validTypes.Add(PermanentUpgradeType.decreaseFoxHungerRate);
                break;

            case PermanentUpgradeCategory.financialUpgrade:
                validTypes.Add(PermanentUpgradeType.decreaseMoveSpeedLeafpointsUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseStartingCapitalUpgrade);
                validTypes.Add(PermanentUpgradeType.increaseLeafpointValueUpgrade);
                break;
        }

        return validTypes.ToArray();
    }

    private string[] GetUpgradeTypeNames(PermanentUpgradeType[] types)
    {
        string[] names = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            names[i] = types[i].ToString();
        }
        return names;
    }
}
