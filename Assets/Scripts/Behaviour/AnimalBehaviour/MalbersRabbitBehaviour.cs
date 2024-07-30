using System.Collections;
using DG.Tweening;
using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Scriptables;
using Managers;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

public class MalbersRabbitBehaviour : MonoBehaviour
{
    [SerializeField] private GrowthManager growthManager;
    [SerializeField] private LeafPointsSpawner leafPointsSpawner;
    [SerializeField] private float hungerThreshold;
    [SerializeField] private float warningThreshold;
    [SerializeField] private float deathThreshold;
    [SerializeField] private ParticleSystem rabbitGhostParticleSystem;
    [SerializeField] private ParticleSystem stoneSpikesParticleSystem;
    [SerializeField] private Material hungryMaterial;
    public BoolVarListener isHungry;
    public MAnimalBrain animal;
    public MAIState deathState;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material rabbitMaterial;
    private bool localIsHungry;
    private bool inWarningState = false;
    private float currentHunger = 0f;

    private void Start()
    {
        isHungry.Value = false;
        localIsHungry = false;
        EntityManager.Get().AddRabbit(gameObject);
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rabbitMaterial = skinnedMeshRenderer.material;
    }

    private void FixedUpdate()
    {
        HandleHunger();
        MaterialChanger();
    }

    private void HandleHunger()
    {
        if (!EnemySpawner.enemyDanger)
        {
            currentHunger += 7.5f * Time.fixedDeltaTime;
        }

        if (currentHunger >= deathThreshold)
            StartCoroutine(Die());

        if (currentHunger >= hungerThreshold && EntityManager.Get().GetFoods().Count >= 1)
        {
            isHungry.Value = true;
            localIsHungry = true;
            CheckFoodDistance();
        }
        if (currentHunger >= warningThreshold && !inWarningState)
        {
            leafPointsSpawner.spawnLeafPoints = false;
            inWarningState = true;
        }
    }

    public void EatFood(GameObject closestFood)
    {
        Food food = closestFood.GetComponent<Food>();
        if (food != null && food.CanBeConsumed())
        {
            food.Consume();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitEat");
            currentHunger -= 100f;
            growthManager.ProgressGrowth(food.foodGrowthValue);
            leafPointsSpawner.spawnLeafPoints = true;
            inWarningState = false;
            isHungry.Value = false;
            localIsHungry = false;
        }
    }

    private void CheckFoodDistance()
    {
        if (animal.Target != null && localIsHungry)
        {
            float distance = Vector3.Distance(transform.position, animal.Target.position);
            if (distance <= 0.75f)
            {
                EatFood(animal.Target.gameObject);
            }
        }
    }

    public void InstantDeath()
    {
        GameManager.animalDeaths++;
        Destroy(gameObject, 3f);
        EntityManager.Get().RemoveRabbit(gameObject);
        if (growthManager.isBaby)
        {
            EntityManager.Get().RemoveBabyRabbit(gameObject);
        }
        if (!rabbitGhostParticleSystem.isPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
            rabbitGhostParticleSystem.Play();
            transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private IEnumerator Die()
    {
        GameManager.animalDeaths++;
        Destroy(gameObject, 3f);
        EntityManager.Get().RemoveRabbit(gameObject);
        if (growthManager.isBaby)
        {
            EntityManager.Get().RemoveBabyRabbit(gameObject);
        }
        animal.StartNewState(deathState);
        yield return new WaitForSeconds(2f);
        if (!rabbitGhostParticleSystem.isPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
            rabbitGhostParticleSystem.Play();
        }
        transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
    }

    private void MaterialChanger()
    {
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.material = inWarningState ? hungryMaterial : rabbitMaterial;
        }
    }

    public void SpawnStoneSpikeParticles()
    {
        stoneSpikesParticleSystem.Play();
    }
}
