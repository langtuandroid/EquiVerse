using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MalbersAnimations.Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrowthManager : MonoBehaviour
{
    public RuntimeGameObjects rabbitGameObjects;
    public ParticleSystem growthParticleSystem;
    
    public int growthProgressValue;
    private int adolescentTreshold;
    private int adultTreshold;
    [HideInInspector]
    public bool isBaby = false;
    [HideInInspector]
    public bool isAdolescent = false;
    [HideInInspector]
    public bool isAdult = false;

    private void Start()
    {
        isBaby = true;
        EntityManager.Get().AddBabyRabbit(gameObject);
        growthProgressValue = 0;
        adolescentTreshold = Random.Range(150, 200);
        adultTreshold = Random.Range(500, 750);

    }

    public void ProgressGrowth(int growthAmount)
    {
        growthProgressValue += growthAmount;
        if (growthProgressValue >= adolescentTreshold && !isAdolescent && !isAdult)
        {   
            isBaby = false;
            EntityManager.Get().RemoveBabyRabbit(gameObject);
            isAdolescent = true;
            rabbitGameObjects.Item_Remove(gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitGrow");
            growthParticleSystem.Play();
            transform.DOScale(0.75f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f);
        }
    
        if (isAdolescent && growthProgressValue >= adultTreshold && !isAdult)
        {
            isAdolescent = false;
            isAdult = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitGrow");
            growthParticleSystem.Play();
            transform.DOScale(1.1f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f);
        }
    }
    
}
