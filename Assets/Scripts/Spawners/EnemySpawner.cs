using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyPrefab
{
    public GameObject enemyPrefab;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
}

public class EnemySpawner : MonoBehaviour
{
    public float initialSpawnDelay;
    public Transform enemySpawnLocation;
    public ParticleSystem portalOpeningParticleSystem;
    public List<EnemyPrefab> enemyPrefabs;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
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
                    portalOpeningParticleSystem.Play();
                    yield return new WaitForSeconds(3f);

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
    }
}
