using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    private Camera mainCamera;
    
    public LayerMask groundLayer;
    public GameObject[] grassPrefabs;  // Prefab for the grass object

    [Header ("GrassCost")]
    public float grassCost = 20f;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
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
}
