using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeVariableController
{
    public static int lowValuePoints = 0;
    public static int highValuePoints = 0;
    public static int gooseEggPoints = 0;
    public static int crystalShardPoints = 0;
    public static int startingPointsBonus = 0;
    public static float hungerThreshold = 0f;
    public static float warningThreshold = 0f;
    public static float deathThreshold = 0;
    public static float eggDropMinWait = 0f;
    public static float eggDropMaxWait = 0f;
    public static float duration = 0f;
    public static float moveSpeed = 0f;
    public static float minTimeTillNextThrow = 0f;
    public static float maxTimeTillNextThrow = 0f;
    public static bool foodQualityUpgrade;
    public static float foxHungerDecrementValue = 0f;

    public static void ResetVariables()
    {
        lowValuePoints = 15;
        highValuePoints = 35;
        gooseEggPoints = 200;
        crystalShardPoints = 200;
        startingPointsBonus = 0;
        hungerThreshold = 100f;
        warningThreshold = 150f;
        deathThreshold = 250f;
        eggDropMinWait = 45f;
        eggDropMaxWait = 60;
        duration = 10f;
        moveSpeed = 1.5f;
        minTimeTillNextThrow = 8;
        maxTimeTillNextThrow = 10;
        foodQualityUpgrade = false;
        foxHungerDecrementValue = 0f;
    }

    public static void SaveVariablesToPlayerPrefs()
{
    PlayerPrefs.SetInt("LowValuePoints", lowValuePoints);
    PlayerPrefs.SetInt("HighValuePoints", highValuePoints);
    PlayerPrefs.SetInt("GooseEggPoints", gooseEggPoints);
    PlayerPrefs.SetInt("CrystalShardPoints", crystalShardPoints);
    PlayerPrefs.SetInt("StartingPointsBonus", startingPointsBonus);
    PlayerPrefs.SetFloat("HungerThreshold", hungerThreshold);
    PlayerPrefs.SetFloat("WarningThreshold", warningThreshold);
    PlayerPrefs.SetFloat("DeathThreshold", deathThreshold);
    PlayerPrefs.SetFloat("EggDropMinWait", eggDropMinWait);
    PlayerPrefs.SetFloat("EggDropMaxWait", eggDropMaxWait);
    PlayerPrefs.SetFloat("Duration", duration);
    PlayerPrefs.SetFloat("MoveSpeed", moveSpeed);
    PlayerPrefs.SetFloat("MinTimeTillNextThrow", minTimeTillNextThrow);
    PlayerPrefs.SetFloat("MaxTimeTillNextThrow", maxTimeTillNextThrow);
    PlayerPrefs.SetInt("FoodQualityUpgrade", foodQualityUpgrade ? 1 : 0);
    PlayerPrefs.SetFloat("FoxHungerDecrementValue", foxHungerDecrementValue);

    PlayerPrefs.Save();
}

public static void LoadVariablesFromPlayerPrefs()
{
    lowValuePoints = PlayerPrefs.GetInt("LowValuePoints", 15);
    highValuePoints = PlayerPrefs.GetInt("HighValuePoints", 35);
    gooseEggPoints = PlayerPrefs.GetInt("GooseEggPoints", 200);
    crystalShardPoints = PlayerPrefs.GetInt("CrystalShardPoints", 200);
    startingPointsBonus = PlayerPrefs.GetInt("StartingPointsBonus", 0);
    hungerThreshold = PlayerPrefs.GetFloat("HungerThreshold", 100f);
    warningThreshold = PlayerPrefs.GetFloat("WarningThreshold", 150f);
    deathThreshold = PlayerPrefs.GetFloat("DeathThreshold", 250f);
    eggDropMinWait = PlayerPrefs.GetFloat("EggDropMinWait", 45f);
    eggDropMaxWait = PlayerPrefs.GetFloat("EggDropMaxWait", 60f);
    duration = PlayerPrefs.GetFloat("Duration", 10f);
    moveSpeed = PlayerPrefs.GetFloat("MoveSpeed", 1.5f);
    minTimeTillNextThrow = PlayerPrefs.GetFloat("MinTimeTillNextThrow", 8f);
    maxTimeTillNextThrow = PlayerPrefs.GetFloat("MaxTimeTillNextThrow", 10f);
    foodQualityUpgrade = PlayerPrefs.GetInt("FoodQualityUpgrade", 0) == 1;
    foxHungerDecrementValue = PlayerPrefs.GetFloat("FoxHungerDecrementValue", 0f);
}

}
