using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwampGolemAttackBehaviour : MonoBehaviour
{
    public Animator animator;

    private bool attacking = false;
    private bool attackCooldown = false;
    private NavMeshAgent navMeshAgent;
    private bool initialSpawnDelayComplete = false;
    private float initialCooldown = 3f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(HandleSpawnDelay());
    }

    private IEnumerator HandleSpawnDelay()
    {
        yield return new WaitForSeconds(initialCooldown);
        initialSpawnDelayComplete = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (initialSpawnDelayComplete && !attacking && !attackCooldown && other.CompareTag("Animal"))
        {
            StartCoroutine(AttackRabbit(other));
        }
    }

    private IEnumerator AttackRabbit(Collider other)
    {
        attacking = true;
        attackCooldown = true;

        navMeshAgent.isStopped = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/Attack");
        animator.SetTrigger("AttackTrigger");

        Vector3 targetPosition = other.transform.position;
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (other != null && other.gameObject.activeSelf)
        {
            MalbersRabbitBehaviour rabbitBehaviour = other.GetComponent<MalbersRabbitBehaviour>();
            if (rabbitBehaviour != null)
            {
                yield return new WaitForSeconds(1f);
                rabbitBehaviour.SpawnStoneSpikeParticles();
                rabbitBehaviour.InstantDeath(false);
            }
        }

        yield return new WaitForSeconds(1.5f);

        navMeshAgent.isStopped = false;
        attacking = false;

        yield return new WaitForSeconds(2f);
        attackCooldown = false;
    }
}
