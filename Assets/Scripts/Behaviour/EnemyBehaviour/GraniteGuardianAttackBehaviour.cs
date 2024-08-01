using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GraniteGuardianAttackBehaviour : MonoBehaviour
{
    public ParticleSystem leftHandParticles;
    public ParticleSystem rightHandParticles;
    public ParticleSystem beam;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    
    private float detectionRadius = 10f;
    private float minimumDistance = 6f;
    private float attackCooldown = 3f;
    private bool attacking = false;
    private bool attackCooldownActive = false;
    private Transform targetTransform;
    private float lastAttackTime;
    private float rotationSpeed = 5f;

    private void Start()
    {
        StartCoroutine(HandleSpawnDelay());
    }

    private IEnumerator HandleSpawnDelay()
    {
        yield return new WaitForSeconds(attackCooldown);
        StartCoroutine(ResetAttackCooldown());
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            RotateTowardsTarget(targetTransform.position);
        }

        if (!attacking && !attackCooldownActive && Time.fixedTime - lastAttackTime >= attackCooldown)
        {
            FindAndSetTarget();
        }
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - beam.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        beam.transform.rotation = Quaternion.Slerp(beam.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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
        animator.SetTrigger("ChargeTrigger");
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/GraniteGuardian/AttackCharge");

        if (initialTarget == null)
        {
            yield return ResetAttack();
            yield break;
        }

        leftHandParticles.Play();
        rightHandParticles.Play();
        yield return new WaitForSeconds(1);

        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(1.5f);

        if (initialTarget == null)
        {
            yield return ResetAttack();
            yield break;
        }

        beam.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/GraniteGuardian/Attack");

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
}
