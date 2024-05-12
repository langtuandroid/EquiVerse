using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DaisyBehaviour : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public Transform spawnPosition;

    public float minSpawnTime, maxSpawnTime;

    private void Start()
    {
        InvokeRepeating("SpawnRabbit", maxSpawnTime, Random.Range(minSpawnTime, maxSpawnTime));
    }

    public void SpawnRabbit()
    {
        transform.DOScale(Vector3.one * 2f, 3f).SetEase(Ease.OutBack).OnComplete((() =>
        {
            transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBack);
            GameObject rabbitInstance = Instantiate(rabbitPrefab, spawnPosition.position, Quaternion.identity);
            rabbitInstance.transform.localScale = Vector3.zero;
            rabbitInstance.transform.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutBack);
            PlaySound("event:/PlayerActions/SpawnAnimal");
            
        }));
    }
    
    private void PlaySound(string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }
}
