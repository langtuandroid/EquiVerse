using UnityEngine;
using UnityEngine.AI;

public class WanderingEnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;

    public float minSpeed = 2.5f;
    public float maxSpeed = 4.5f;
    public float smoothRotationSpeed = 5f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the GameObject.");
            enabled = false; // Disable the script if NavMeshAgent is not found.
        }

        SetRandomDestination();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1f || !navMeshAgent.hasPath)
        {
            SetRandomDestination();
        }

        MoveAndRotateTowardsDestination();
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
        targetPosition = hit.position;

        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        navMeshAgent.speed = randomSpeed;

        // Set the new destination
        navMeshAgent.SetDestination(targetPosition);
    }

    private void MoveAndRotateTowardsDestination()
    {
        // Smooth rotation towards the destination
        if (navMeshAgent.hasPath)
        {
            Quaternion targetRotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotationSpeed);
        }
    }
}