using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using MalbersAnimations;
using UnityEngine;
using Random = UnityEngine.Random;

public class DaisyBehaviour : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public Transform spawnPosition;

    public float minSpawnTime, maxSpawnTime;
    
    public FMODUnity.EventReference expandingSound, rabbitBornSound, spawnAnimalSound;

    public BoolVarListener isPregnant;

    public ParticleSystem rabbitBornParticles;

    private void Start()
    {
        isPregnant.Value = false;
        InvokeRepeating("SpawnRabbit", maxSpawnTime, Random.Range(minSpawnTime, maxSpawnTime));
    }

    public void SpawnRabbit()
    {
        isPregnant.Value = true;
        transform.DOScale(Vector3.one * 2.3f, 8f).SetEase(Ease.OutBack).OnComplete((() =>
        {
            transform.DOScale(Vector3.one * 1.5f, 1f).SetEase(Ease.OutBack);
            GameObject rabbitInstance = Instantiate(rabbitPrefab, spawnPosition.position, Quaternion.identity);
            rabbitInstance.transform.localScale = Vector3.zero;
            rabbitInstance.transform.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutBack);
            rabbitBornParticles.transform.position = rabbitInstance.transform.position;
            rabbitBornParticles.Play();
            PlaySound(spawnAnimalSound);
            PlaySound(rabbitBornSound);
            isPregnant.Value = false;
            
        }));
    }
    
    private void PlaySound(EventReference eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }
}
