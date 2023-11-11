using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WanderScript : MonoBehaviour
{
    public float wanderRadius = 10f;
    public Vector2 wanderTimerRange = new Vector2(3f, 7f);
    public Vector2 idleDurationRange = new Vector2(2f, 5f);
    public Vector2 speedRange = new Vector2(0.5f, 1.0f);
    public float searchRange = 10f;
    public float hungerThreshold = 50f;
    public float deathThreshold = 100f;
    public Material hungryMaterial;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private NavMeshAgent agent;
    private Animator animator;
    private Material rabbitMaterial;

    private float wanderTimer;
    private float idleDuration;
    private float speed;
    private float stateTimer;
    private bool isIdling = false;
    private bool isHungry = false;
    private float currentHunger = 0f;

    private void Start()
    {
        InitializeComponents();
        Idle();
    }

    private void FixedUpdate()
    {
        MaterialChanger();
        HandleHunger();

        if (isHungry)
            FindClosestPlant();
        else
            HandleWanderAndIdle();
    }

    private void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rabbitMaterial = skinnedMeshRenderer.material;
    }

    private void HandleHunger()
    {
        currentHunger += 5f * Time.fixedDeltaTime;
        
        if (currentHunger >= deathThreshold)
            StartCoroutine(Die());

        if (currentHunger >= hungerThreshold)
        {
            isHungry = true;
            LeafPointsSpawner.spawnLeafPoints = false;
        }
    }

    private void HandleWanderAndIdle()
    {
        animator.SetBool("isLookingOut", false);
        if (isIdling)
        {
            stateTimer -= Time.fixedDeltaTime;

            if (stateTimer <= 0f)
            {
                isIdling = false;
                Wander();
                animator.SetBool("isJumping", true);
            }
        }
        else
        {
            stateTimer -= Time.fixedDeltaTime;

            if (stateTimer <= 0f)
            {
                isIdling = true;
                Idle();
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void Wander()
    {
        wanderTimer = Random.Range(wanderTimerRange.x, wanderTimerRange.y);
        idleDuration = 0f;
        stateTimer = wanderTimer;

        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        agent.SetDestination(navHit.position);

        speed = Random.Range(speedRange.x, speedRange.y);
        agent.speed = speed;
    }

    private void Idle()
    {
        agent.speed = 0;
        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
        stateTimer = idleDuration;
    }

    private void FindClosestPlant()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        if (plants.Length == 0)
            return;

        Transform closestPlant = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject plant in plants)
        {
            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlant = plant.transform;
            }
        }

        if (closestPlant != null)
        {
            if (closestDistance > 0.15f)
            {
                agent.speed = 1.3f;
                agent.SetDestination(closestPlant.position);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
                Destroy(closestPlant.gameObject);
                PlantSpawner.RemovePlant();
                currentHunger = 0f;
                LeafPointsSpawner.spawnLeafPoints = true;
                isHungry = false;
                animator.SetBool("isLookingOut", true);
            }
        }
    }

    private void MaterialChanger()
    {
        skinnedMeshRenderer.material = isHungry ? hungryMaterial : rabbitMaterial;
    }

    private IEnumerator Die()
    {
        agent.speed = 0;
        animator.SetBool("isJumping", false);
        animator.SetBool("isDead_0", true);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
