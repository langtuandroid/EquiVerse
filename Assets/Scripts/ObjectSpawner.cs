using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public LayerMask groundLayer;

    public GameObject[] grassPrefabs;  // Prefab for the grass object
    public GameObject rabbitPrefab;

    private float grassCost = 2f;
    private float rabbitCost = 5f;

    [HideInInspector]
    public static float generationValue = 0f;
    
    private float grassGenerateValue = 0.1f;
    private float rabbitGenerateValue = 1.2f;


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
                if (grassSelected && ECManager.totalPoints >= grassCost)
                {
                    int randomIndex = Random.Range(0, grassPrefabs.Length);
                    GameObject randomPrefab = grassPrefabs[randomIndex];
                    // Instantiate the grass prefab at the clicked position
                    GameObject spawnedPrefab = Instantiate(randomPrefab, hit.point, Quaternion.identity);
                    ECManager.totalPoints -= grassCost;
                    generationValue += grassGenerateValue;
                }else if (rabbitSelected && ECManager.totalPoints >= rabbitCost)
                {
                    GameObject spawnedPrefab = Instantiate(rabbitPrefab, hit.point, Quaternion.identity);
                    ECManager.totalPoints -= rabbitCost;
                    generationValue += rabbitGenerateValue;
                }
            }
        }
    }
}
