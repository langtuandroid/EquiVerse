using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    [Header("SpawnPrefabs")]
    public GameObject rabbitPrefab;

    [Header ("SpawnCost")]
    public float rabbitCost = 100f;

    public Transform planeTransform;

    private int amountOfRabbits;

    public GameObject gameOverPopUp;

    public void SpawnRabbit()
    {
        if (ECManager.totalPoints < rabbitCost)
        {
            FindAliveRabbits();
            if (amountOfRabbits <= 0)
            {
                gameOverPopUp.SetActive(true);
                this.gameObject.SetActive(false);
            }
            return;
        }

        ECManager.totalPoints -= rabbitCost;

        Vector3 randomPosition = GetRandomPositionOnNavigationMesh();

        if (randomPosition != Vector3.zero)
        {
            Instantiate(rabbitPrefab, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to find a valid position for the rabbit.");
        }
    }

    private Vector3 GetRandomPositionOnNavigationMesh()
    {
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;
        int maxAttempts = 10;

        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            randomPosition = new Vector3(
                Random.Range(planeTransform.position.x - planeTransform.localScale.x / 2, planeTransform.position.x + planeTransform.localScale.x / 2),
                planeTransform.position.y,
                Random.Range(planeTransform.position.z - planeTransform.localScale.z / 2, planeTransform.position.z + planeTransform.localScale.z / 2)
            );

            if (NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }

    private void FindAliveRabbits()
    {
        GameObject[] rabbits;
        rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        amountOfRabbits = rabbits.Length;
    }
}
