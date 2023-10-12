using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
using System.Collections;

public class RabbitBehaviour : MonoBehaviour
{
    public float searchRange = 10f;  // The range in which the rabbit can detect plants.
    public float hungerThreshold = 50f; // The threshold at which the rabbit becomes hungry.
    public float deathThreshold = 100f; // The threshold at which the rabbit dies.

    private Animal_WanderScript animalWander;
    private NavMeshAgent agent;
    private bool isHungry = false;
    private float currentHunger = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animalWander = GetComponent<Animal_WanderScript>();
    }

    void FixedUpdate()
    {
        currentHunger += 0f * Time.fixedDeltaTime;

        if (currentHunger >= hungerThreshold)
        {
            isHungry = true;
        }
        
        if (isHungry)
        {
            FindClosestPlant();
            if (currentHunger >= deathThreshold)
            {
                StartCoroutine(Die());
            }
        }
    }

    void FindClosestPlant()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        if (plants.Length == 0)
        {
            Debug.Log("No plants found.");
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
            Debug.Log("Closest plant found. Distance: " + closestDistance);
            agent.SetDestination(closestPlant.position);

            if (closestDistance < 1f) // Assuming the rabbit eats the plant when very close.
            {
                Destroy(closestPlant.gameObject);
                currentHunger = 0f;
                isHungry = false;
            }
        }
    }
   
    private IEnumerator Die()
    {
        animalWander.Die();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
