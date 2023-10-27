using System;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Random = UnityEngine.Random;

public class LeafPointsSpawner : MonoBehaviour
{
    public GameObject leafPointPrefab;
    private float spawnTimer = 0f;
    private float timeBetweenLeafSpawn;
    public Vector2 timeBetweenLeafSpawnRange = new Vector2(10f, 18f);
    private float desiredHeight = 5f;
    private float duration = 5f;

    public static bool spawnLeafPoints = true;

    private void Start()
    {
        timeBetweenLeafSpawn = Random.Range(timeBetweenLeafSpawnRange.x, timeBetweenLeafSpawnRange.y);
    }

    private void FixedUpdate()
    {
        if (spawnLeafPoints)
        {
            spawnTimer += Time.fixedDeltaTime;
            if (spawnTimer >= timeBetweenLeafSpawn)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0); // Use the position of the object
                GameObject newLeaf = Instantiate(leafPointPrefab, spawnPosition, Quaternion.identity);

                // Use DoTween to move the object to the desired height
                newLeaf.transform.DOMoveY(desiredHeight, duration).SetEase(Ease.OutSine)
                    .OnComplete(() => FadeAndDestroy(newLeaf));

                spawnTimer = 0f;
                timeBetweenLeafSpawn = Random.Range(timeBetweenLeafSpawnRange.x, timeBetweenLeafSpawnRange.y);
            }
        }
    }

    private void FadeAndDestroy(GameObject obj)
    {
        Material material = obj.GetComponent<Renderer>().material;
        material.DOFade(0f, duration).OnComplete(() => Destroy(obj));
    }
}
