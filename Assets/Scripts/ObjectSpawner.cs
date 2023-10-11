using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public LayerMask groundLayer;

    [Header("SpawnPrefabs")]
    public GameObject[] grassPrefabs;  // Prefab for the grass object
    public GameObject rabbitPrefab;

    [Header ("SpawnCost")]
    public float rabbitCost = 100f;
    public float grassCost = 20f;

    [HideInInspector]
    public static float generationValue = 0f;
    
    private float rabbitGenerateValue = 1f;

    public Transform planeTransform;

    private Camera mainCamera;

    [HideInInspector]
    public static bool grassSelected = false, rabbitSelected = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast to detect the ground
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                if (ECManager.totalPoints >= grassCost && grassPrefabs.Length < 5)
                {
                    int randomIndex = Random.Range(0, grassPrefabs.Length);
                    GameObject randomPrefab = grassPrefabs[randomIndex];
                    // Instantiate the grass prefab at the clicked position
                    GameObject spawnedPrefab = Instantiate(randomPrefab, hit.point, Quaternion.identity);
                    ECManager.totalPoints -= grassCost;
                }
            }
        }
    }

    public void SpawnRabbit()
    {
        if (ECManager.totalPoints >= rabbitCost)
        {
            ECManager.totalPoints -= rabbitCost;
            generationValue += rabbitGenerateValue;

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
