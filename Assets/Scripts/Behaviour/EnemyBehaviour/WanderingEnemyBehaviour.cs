using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WanderingEnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float speed = 3f;
    public float smoothRotationSpeed = 360f;

    private NavMeshSurface navMeshSurface;
    
    private void Start()
    {
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navMeshSurface = GetNavMeshSurfaceByAgentType(navMeshAgent.agentTypeID);
        navMeshAgent.speed = speed;
        SetRandomDestination();
    }

    private void FixedUpdate()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, navMeshSurface.layerMask))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }
    
    private NavMeshSurface GetNavMeshSurfaceByAgentType(int agentTypeID)
    {
        NavMeshSurface[] navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
        foreach (NavMeshSurface surface in navMeshSurfaces)
        {
            if (surface.agentTypeID == agentTypeID)
            {
                return surface;
            }
        }
        return null;
    }
}

