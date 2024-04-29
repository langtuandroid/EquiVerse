using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Scriptables;
using Spawners;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MalbersRabbitBehaviour : MonoBehaviour
{
    [SerializeField] private GrowthManager growthManager;
    [SerializeField] private float hungerThreshold;
    [SerializeField] private float warningThreshold;
    [SerializeField] private float deathThreshold;
    [SerializeField] private ParticleSystem rabbitGhostParticleSystem;
    [SerializeField] private Material hungryMaterial;
    public BoolVarListener isHungry;
    public MAnimalBrain animal;
    public MAIState deathState;
    public NavMeshAgent agent;
    private Material rabbitMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private bool localIsHungry;
    private MAnimalBrain animalBrain;
    private bool inWarningState = false;
    private float currentHunger = 0f;


    private void Start()
    {
        isHungry.Value = false;
        localIsHungry = false;
        EntityManager.Get().AddRabbit(gameObject);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        
        rabbitMaterial = skinnedMeshRenderer.material;
        
    }

    private void FixedUpdate()
    {
        HandleHunger();
        MaterialChanger();
    }

    private void HandleHunger() {
        currentHunger += 5f * Time.fixedDeltaTime;

        if (currentHunger >= deathThreshold)
            StartCoroutine(Die());

        if (currentHunger >= hungerThreshold)
        {
            isHungry.Value = true;
            localIsHungry = true;

            if (currentHunger >= warningThreshold) {
                LeafPointsSpawner.spawnLeafPoints = false;
                inWarningState = true;
            }
        }
    }
    
    public void EatFood(GameObject closestFood) {
        Food food = closestFood.GetComponent<Food>(); 
        food.Consume(); 
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitEat");
        currentHunger -= 100f;
        growthManager.ProgressGrowth(food.foodGrowthValue);
        LeafPointsSpawner.spawnLeafPoints = true;
        inWarningState = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plant") && localIsHungry)
        {
            isHungry.Value = false;
            localIsHungry = false;
            EatFood(other.gameObject);
        }
    }
    
    public void InstantDeath()
    {
        Destroy(gameObject, 3f);
        if (!rabbitGhostParticleSystem.isPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
            rabbitGhostParticleSystem.Play();
            transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private IEnumerator Die()
    {
        Destroy(gameObject, 3f);
        animal.StartNewState(deathState);
        yield return new WaitForSeconds(2f);
        if (!rabbitGhostParticleSystem.isPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
            rabbitGhostParticleSystem.Play();
        }
        transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
    }
    
    private void MaterialChanger() {
        skinnedMeshRenderer.material = inWarningState ? hungryMaterial : rabbitMaterial;
    }
}
