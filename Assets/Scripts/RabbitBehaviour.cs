using UnityEngine;
using UnityEngine.AI;

public class RabbitBehaviour : MonoBehaviour
{
    public float startingHunger = 0f; // The initial hunger level
    public float hungerIncreaseRate = 1f; // The rate at which hunger increases per second
    public float eatingPower = 10f; // The amount of hunger reduced when the rabbit eats from a plant

    private float currentHunger; // The current hunger level
    private bool isHungry = false;
    private GameObject targetPlant;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        currentHunger = startingHunger;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is a plant
        Plant plant = other.GetComponent<Plant>();
        if (plant != null)
        {
            // Check if the rabbit is hungry and there is no current target plant
            if (isHungry && targetPlant == null)
            {
                // Set the plant as the target plant
                targetPlant = other.gameObject;
                MoveToTargetPlant();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider is the current target plant
        if (other.gameObject == targetPlant)
        {
            // Reset the target plant
            targetPlant = null;
        }
    }

    private void Update()
    {
        print(currentHunger);
        // Increase hunger over time
        currentHunger += hungerIncreaseRate * Time.deltaTime;

        // Check the rabbit's hunger level
        if (isHungry)
        {
            // If there is no target plant, find the closest plant and set it as the target plant
            if (targetPlant == null)
            {
                FindClosestPlant();
                if (targetPlant != null)
                {
                    MoveToTargetPlant();
                }
            }
            else
            {
                // If there is a target plant, move towards it using the NavMeshAgent
                if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    // The rabbit has reached the destination or has no path, so reset the target plant
                    targetPlant = null;
                }
            }
        }
        else
        {
            // The rabbit is not hungry, no need to search for plants
            // You can implement other behaviors for the rabbit here
        }

        // Check if the rabbit is hungry based on the current hunger level
        if (currentHunger >= 50f)
        {
            SetHungry(true);
        }
    }

    public void SetHungry(bool hungry)
    {
        isHungry = hungry;

        if (isHungry)
        {
            // Reset the target plant when the rabbit gets hungry
            targetPlant = null;
            FindClosestPlant();
            if (targetPlant != null)
            {
                MoveToTargetPlant();
            }
        }
    }

    public float GetEatingPower()
    {
        return eatingPower;
    }

    public void ReduceHunger(float amount)
    {
        currentHunger -= amount;

        if (currentHunger < 0f)
        {
            currentHunger = 0f;
        }
    }

    private void MoveToTargetPlant()
    {
        navMeshAgent.SetDestination(targetPlant.transform.position);
        navMeshAgent.isStopped = false; // Ensure the NavMeshAgent is not stopped
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
}
