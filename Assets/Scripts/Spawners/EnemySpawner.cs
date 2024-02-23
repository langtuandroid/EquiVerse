using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[System.Serializable]
public class EnemyPrefab
{
    public GameObject enemyPrefab;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
}

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;
    public World1LevelSoundController soundController;
    
    public float initialSpawnDelay;
    public Transform enemySpawnLocation;
    public ParticleSystem portalOpeningParticleSystem;
    public List<EnemyPrefab> enemyPrefabs;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public TextMeshProUGUI newEnemyTypeWarningText;

    private void Start()
    {
        if (!gameManager.secondLevelTutorialActivated)
        {
            SpawnEnemies();
        }
    }


    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesWithDelays());
    }

    IEnumerator SpawnEnemiesWithDelays()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            foreach (var enemyPrefab in enemyPrefabs)
            {
                if (enemyPrefab.enemyPrefab != null)
                {
                    newEnemyTypeWarningText.gameObject.SetActive(true);
                    newEnemyTypeWarningText.text = "Alert! Portal fluctuations indicate a <b>Swamp Golem</b> is about to gatecrash the party!";
                    
                    soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 1.2f);
                    soundController.StartAudioEvent("BattleMusic");
                    soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 1f, 1.2f);
                    
                    yield return new WaitForSeconds(10f);
                    newEnemyTypeWarningText.gameObject.SetActive(false);
                    portalOpeningParticleSystem.Play();
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SpawnPortal/SpawnPortalCharge");
                    yield return new WaitForSeconds(3f);
                    
                    soundController.FadeAudioParameter("BattleMusic", "Suspense_Action_Transition", 1f, 1f);
                    GameObject newEnemy = Instantiate(enemyPrefab.enemyPrefab, enemySpawnLocation.position, Quaternion.identity);
                    activeEnemies.Add(newEnemy); // Add the newly instantiated enemy to the active list.

                    portalOpeningParticleSystem.Stop();

                    float spawnDelay = Random.Range(enemyPrefab.minSpawnDelay, enemyPrefab.maxSpawnDelay);
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    Debug.LogWarning("An enemy prefab in the list is not assigned.");
                }
            }
        }
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
            Destroy(enemyToRemove); // Destroy the enemy game object.
        }
        
        if (activeEnemies.Count == 0)
        {
            soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 0f, 1.2f);
            soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 1f, 1.2f);
        }
    }
}
