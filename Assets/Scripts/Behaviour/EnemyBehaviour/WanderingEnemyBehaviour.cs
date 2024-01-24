using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WanderingEnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;


    public Animator animator;
    public float speed;
    public float smoothRotationSpeed = 5f;

    private bool attacking = false;

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
        
        if (WanderingEnemyFXController.attacking)
        {
            navMeshAgent.speed = 0;
        }
        else
        {
            navMeshAgent.speed = speed;
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
        targetPosition = hit.position;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rabbit"))
        {
            WanderingEnemyFXController.attacking = true;
            animator.SetTrigger("AttackTrigger");
            other.GetComponent<WanderScript>().Die();
        }
    }
}