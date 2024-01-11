using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Make sure to include this namespace

[System.Serializable]
public class EnemyPrefabWithDelay
{
    public GameObject enemyPrefab;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public float launchDistance = 2f; // Adjust this value as needed
}

public class EnemySpawner : MonoBehaviour
{
    public Transform enemySpawnLocation;
    public ParticleSystem portalOpeningParticleSystem;
    public List<EnemyPrefabWithDelay> enemyPrefabsWithDelays;

    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelays());
    }

    IEnumerator SpawnEnemiesWithDelays()
    {
        while (true)
        {
            foreach (var enemyPrefabWithDelay in enemyPrefabsWithDelays)
            {
                if (enemyPrefabWithDelay.enemyPrefab != null)
                {
                    portalOpeningParticleSystem.Play();
                    yield return new WaitForSeconds(3f);

                    GameObject newEnemy = Instantiate(enemyPrefabWithDelay.enemyPrefab, enemySpawnLocation.position, Quaternion.identity);

                    // Move the NavMeshAgent a short distance
                    NavMeshAgent enemyNavMeshAgent = newEnemy.GetComponent<NavMeshAgent>();
                    if (enemyNavMeshAgent != null)
                    {
                        Vector3 destination = enemySpawnLocation.position + enemySpawnLocation.forward * enemyPrefabWithDelay.launchDistance;
                        enemyNavMeshAgent.Warp(enemySpawnLocation.position); // Set the initial position
                        enemyNavMeshAgent.SetDestination(destination);
                    }

                    portalOpeningParticleSystem.Stop();

                    float spawnDelay = Random.Range(enemyPrefabWithDelay.minSpawnDelay, enemyPrefabWithDelay.maxSpawnDelay);
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    Debug.LogWarning("An enemy prefab in the list is not assigned.");
                }
            }
        }
    }
}


