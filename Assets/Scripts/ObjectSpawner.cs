using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("SpawnPrefabs")]
    public GameObject rabbitPrefab;

    [Header ("SpawnCost")]
    public float rabbitCost = 100f;

    public Transform planeTransform;

    public void SpawnRabbit()
    {
        if (ECManager.totalPoints >= rabbitCost)
        {
            ECManager.totalPoints -= rabbitCost;

            // Get the bounds of the plane
            Renderer planeRenderer = planeTransform.GetComponent<Renderer>();
            Vector3 planeCenter = planeRenderer.bounds.center;
            Vector3 planeExtents = planeRenderer.bounds.extents;

            // Calculate the random position within the plane bounds
            Vector3 randomPosition = new Vector3(
                Random.Range(planeCenter.x - planeExtents.x, planeCenter.x + planeExtents.x),
                planeCenter.y,
                Random.Range(planeCenter.z - planeExtents.z, planeCenter.z + planeExtents.z)
            );

            // Perform a raycast to find the actual ground position at the random position
            RaycastHit hit;
            if (Physics.Raycast(randomPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity))
            {
                randomPosition = hit.point;
            }

            // Instantiate the rabbit prefab at the random position
            Instantiate(rabbitPrefab, randomPosition, Quaternion.identity);
        }
    }
}
