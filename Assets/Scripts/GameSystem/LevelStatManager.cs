using System.Collections.Generic;
using UnityEngine;

public class LevelStat
{
    public string statName;
    public string statValue;
}

public class LevelStatManager : MonoBehaviour
{
    public List<LevelStat> levelStats;

    private void Start()
    {
        InitializeLevelStats();
    }

    private void InitializeLevelStats()
    {
        levelStats = new List<LevelStat>
        {
            new LevelStat { statName = "Completion Time", statValue = "00:00" },
            new LevelStat { statName = "Leaf Points Collected", statValue = "0" },
            new LevelStat { statName = "Animals Spawned", statValue = "0" },
            new LevelStat {statName = "Animals Deaths", statValue = "0"}
        };
    }

    public List<LevelStat> GetLevelStats()
    {
        return levelStats;
    }

    public void UpdateStat(string statName, string statValue)
    {
        var stat = levelStats.Find(s => s.statName == statName);
        if (stat != null)
        {
            stat.statValue = statValue;
        }
        else
        {
            levelStats.Add(new LevelStat { statName = statName, statValue = statValue });
        }
    }
}