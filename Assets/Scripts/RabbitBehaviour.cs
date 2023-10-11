using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
using System.Collections;

public class RabbitBehaviour : MonoBehaviour
{
    public float startingHunger = 0f;
    public float hungerIncreaseRate = 1f;

    private float currentHunger;
    public bool isHungry;
    public float hungerThreshold = 50;
    public float starvationThreshold = 150;
    private GameObject targetPlant;
    private NavMeshAgent navMeshAgent;
    private Animal_WanderScript animal_wander;

    private void Start()
    {
        currentHunger = startingHunger;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animal_wander = GetComponent<Animal_WanderScript>();
        navMeshAgent.stoppingDistance = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHungry && targetPlant == null)
        {
            Plant plant = other.GetComponent<Plant>();
            if (plant != null)
            {
                targetPlant = other.gameObject;
                navMeshAgent.isStopped = true;
                MoveToTargetPlant();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetPlant)
        {
            targetPlant = null;
            MoveToNextClosestPlant();
        }
    }

    private void FixedUpdate()
    {
        print(currentHunger);
        currentHunger += hungerIncreaseRate * Time.fixedDeltaTime;

        if (isHungry)
        {
            if (targetPlant == null)
            {
                FindClosestPlant();
                MoveToTargetPlant();
            }
            else if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                targetPlant = null;
                MoveToNextClosestPlant();
            }
        }

        if (currentHunger >= hungerThreshold)
        {
            SetHungry(true);
            if (currentHunger >= starvationThreshold)
            {
                StartCoroutine(Death());
            }
        }
    }

    public void SetHungry(bool hungry)
    {
        isHungry = hungry;

        if (isHungry)
        {
            targetPlant = null;
            FindClosestPlant();
            MoveToTargetPlant();
        }
    }

    public void ReduceHunger(float amount)
    {
        currentHunger = Mathf.Max(0f, currentHunger - amount);
    }

    private void MoveToTargetPlant()
    {
        if (targetPlant != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPlant.transform.position);
            StartCoroutine(MonitorMovement()); // Start coroutine to monitor movement towards the target plant
        }
    }

    private IEnumerator MonitorMovement()
    {
        while (targetPlant != null && navMeshAgent.pathPending)
        {
            yield return null;
        }

        // Rabbit has reached the destination or has no path, so reset the target plant
        targetPlant = null;
        MoveToNextClosestPlant();
    }

    private void MoveToNextClosestPlant()
    {
        if (isHungry && targetPlant == null)
        {
            FindClosestPlant();
            MoveToTargetPlant();
        }
    }

    private void FindClosestPlant()
    {
        Plant[] plants = GameObject.FindObjectsOfType<Plant>();
        float closestDistance = Mathf.Infinity;

        foreach (Plant plant in plants)
        {
            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPlant = plant.gameObject;
            }
        }
    }

    private IEnumerator Death()
    {
        animal_wander.Die();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
