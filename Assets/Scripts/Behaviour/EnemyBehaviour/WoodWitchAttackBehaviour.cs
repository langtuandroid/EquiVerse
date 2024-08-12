using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WoodWitchAttackBehaviour : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GameObject child;
    public float speed;
    public float smoothRotationSpeed;

    private NavMeshSurface navMeshSurface;
    private int clicksBeforeWarping = 2;
    private int clickCount = 0;
    private float warpBackTimer = 5f;
    private float warpCounter = 0f;
    private float detectionRadius = 10f;
    private float minimumDistance = 6f; // Minimum distance to target rabbits
    private float attackCooldown = 3f;
    private bool warped = false;
    private bool attacking = false;
    private bool attackCooldownActive = false;
    private Transform targetTransform;
    private float lastAttackTime;
    private float rotationSpeed = 5f; // Rotation speed factor

    public void WitchClicked()
    {
        clickCount++;
        if (clickCount >= clicksBeforeWarping)
        {
            warped = true;
            clickCount = 0;
            child.SetActive(false);
            warpCounter = 0f;
        }
    }

    private void Start()
    {
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navMeshSurface = GetNavMeshSurfaceByAgentType(navMeshAgent.agentTypeID);
        navMeshAgent.speed = speed;
        StartCoroutine(ResetAttackCooldown());
        WarpWitch();
    }

    private void FixedUpdate()
    {
        if (!warped)
        {
            if (targetTransform != null)
            {
                RotateTowardsTarget(targetTransform.position);
            }

            if (!attacking && !attackCooldownActive && Time.time - lastAttackTime >= attackCooldown)
            {
                FindAndSetTarget();
            }
        }
        else
        {
            warpCounter += Time.fixedDeltaTime;
            if (warpCounter >= warpBackTimer)
            {
                child.SetActive(true);
                warped = false;
                WarpWitch();
            }
        }
    }

    private void WarpWitch()
    {
        // Play a cool warping effect, sound effects, etc
        navMeshAgent.Warp(GetRandomNavMeshPoint());
    }

    private Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10f;
        randomPoint += navMeshSurface.transform.position;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        Debug.LogError("GetRandomNavMeshPoint was unable to find a point within the surface and returned Zero! Please report this");
        return Vector3.zero;
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void FindAndSetTarget()
    {
        List<GameObject> rabbits = EntityManager.Get().GetRabbits();
        GameObject closestRabbit = null;
        float closestDistance = detectionRadius;

        foreach (GameObject rabbit in rabbits)
        {
            float distance = Vector3.Distance(transform.position, rabbit.transform.position);
            if (distance <= closestDistance && distance >= minimumDistance)
            {
                closestRabbit = rabbit;
                closestDistance = distance;
            }
        }

        if (closestRabbit != null)
        {
            targetTransform = closestRabbit.transform;
            StartCoroutine(AttackTarget(closestRabbit));
        }
    }

    private IEnumerator AttackTarget(GameObject initialTarget)
    {
        attacking = true;
        attackCooldownActive = true;
        navMeshAgent.isStopped = true;
        // Uncomment if there's an attack animation trigger to set
        // animator.SetTrigger("Attack");

        if (initialTarget == null)
        {
            yield return ResetAttack();
            yield break;
        }

        // Uncomment if there's an attack animation trigger to set
        // animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);

        if (initialTarget == null)
        {
            yield return ResetAttack();
            yield break;
        }

        MalbersRabbitBehaviour rabbitBehaviour = initialTarget.GetComponent<MalbersRabbitBehaviour>();
        if (rabbitBehaviour != null)
        {
            yield return new WaitForSeconds(2f);
            if (initialTarget != null && initialTarget.activeSelf)
            {
                rabbitBehaviour.InstantDeath(false);
            }
        }

        yield return new WaitForSeconds(2f);

        yield return ResetAttack();
    }

    private IEnumerator ResetAttack()
    {
        targetTransform = null;
        navMeshAgent.isStopped = false;
        attacking = false;
        lastAttackTime = Time.time;
        yield return new WaitForSeconds(attackCooldown);
        attackCooldownActive = false;
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackCooldownActive = false;
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
