using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    public ParticleSystem growthParticleSystem;
    
    private int growthProgressValue;
    private int adolescentTreshold = 150;
    private int adultTreshold = 500;
    private int plantGrowthValue = 50;
    [HideInInspector]
    public bool isAdolescent = false;
    [HideInInspector]
    public bool isAdult = false;

    private void Start()
    {
        growthProgressValue = 0;
    }

    public void ProgressGrowth()
    {
        growthProgressValue += plantGrowthValue;
    
        if (growthProgressValue >= adolescentTreshold && !isAdolescent)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitGrow");
            growthParticleSystem.Play();
            transform.DOScale(0.75f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f);
            isAdolescent = true;
        }
    
        if (isAdolescent && growthProgressValue >= adultTreshold && !isAdult)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitGrow");
            growthParticleSystem.Play();
            transform.DOScale(1.1f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f);
            isAdult = true;
        }
    }
    
}
