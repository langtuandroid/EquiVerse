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

    public float searchRange = 10f;  // The range in which the rabbit can detect plants.
    public float hungerThreshold = 50f; // The threshold at which the rabbit becomes hungry.
    public float deathThreshold = 100f; // The threshold at which the rabbit dies;
    
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private NavMeshAgent agent;
    private float wanderTimer;
    private float idleDuration;
    private float speed;
    private float stateTimer;
    private bool isIdling = false;
    private bool isHungry = false;
    private float currentHunger = 0f;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Wander();
    }

    void FixedUpdate()
    {
        HandleHunger();

        if (isHungry)
        {
            FindClosestPlant();
        }
        else
        {
            HandleWanderAndIdle();
        }
        
        print(agent.speed);
    }

    void HandleHunger()
    {
        currentHunger += 5f * Time.fixedDeltaTime;

        Material newMaterial = Instantiate(skinnedMeshRenderer.material);
        skinnedMeshRenderer.material = newMaterial;
        
        if (currentHunger >= hungerThreshold)
        {
            isHungry = true;
            newMaterial.DOColor(Color.red * 0.5f, "_EmissionColor", 0.1f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            newMaterial.SetColor("_EmissionColor", Color.black); // Reset emission when not hungry.
        }
        if (currentHunger >= deathThreshold)
        { 
            StartCoroutine(Die());
        }
        
    }

    void HandleWanderAndIdle()
    {
        if (isIdling)
        {
            idleDuration -= Time.fixedDeltaTime;

            if (idleDuration <= 0f)
            {
                isIdling = false;
                Wander();
            }
        }
        else
        {
            stateTimer -= Time.fixedDeltaTime;

            if (stateTimer <= 0f)
            {
                isIdling = true;
                Idle();
            }
        }

        animator.SetBool("isJumping", !isIdling);
    }

    void Wander()
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

    void Idle()
    {
        agent.speed = 0;
        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
        stateTimer = idleDuration;
    }

    void FindClosestPlant()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        if (plants.Length == 0)
        {
            return;
        }

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
            if (closestDistance > 1f)
            {
                agent.speed = 1.3f;
                agent.SetDestination(closestPlant.position);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
                Destroy(closestPlant.gameObject);
                currentHunger = 0f;
                isHungry = false;
            }
        }
    }

    private IEnumerator Die()
    {
        // Handle the die behavior
        agent.speed = 0;
        animator.SetBool("isJumping", false);
        animator.SetBool("isDead_1", true);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

