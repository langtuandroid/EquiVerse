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
    [HideInInspector] public float detectionRadius = 10f;
    [HideInInspector] public float attackCooldown = 2f;
    private bool attacking = false;
    private bool attackCooldownActive = false;
    private GameObject target;
    private float lastAttackTime;

    private void Start()
    {
        attackCooldownActive = true;
        StartCoroutine(ResetAttackCooldown());
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - beam.transform.position;
            beam.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (!attacking && !attackCooldownActive && Time.time - lastAttackTime >= attackCooldown)
        {
            List<GameObject> rabbits = EntityManager.Get().GetRabbits();

            foreach (GameObject rabbit in rabbits)
            {
                float distance = Vector3.Distance(transform.position, rabbit.transform.position);
                if (distance <= detectionRadius)
                {
                    target = rabbit;
                    StartCoroutine(AttackTarget(target));
                    break;
                }
            }
        }
    }

    private IEnumerator AttackTarget(GameObject initialTarget)
    {
        attacking = true;
        attackCooldownActive = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("ChargeTrigger");
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/GraniteGuardian/AttackCharge");
        
        if (initialTarget == null) // Check if the target still exists
        {
            ResetAttackState();
            yield break;
        }

        Vector3 targetDirection = initialTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = targetRotation;

        leftHandParticles.Play();
        rightHandParticles.Play();
        yield return new WaitForSeconds(1);
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(1.5f);

        if (initialTarget == null) // Check again before accessing target properties
        {
            ResetAttackState();
            yield break;
        }

        Vector3 direction = initialTarget.transform.position - beam.transform.position;
        beam.transform.rotation = Quaternion.LookRotation(direction);
        beam.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/GraniteGuardian/Attack");

        if (initialTarget != null && initialTarget.gameObject.activeSelf)
        {
            MalbersRabbitBehaviour rabbitBehaviour = initialTarget.GetComponent<MalbersRabbitBehaviour>();
            if (rabbitBehaviour != null)
            {
                yield return new WaitForSeconds(2f); // Adjust the delay as needed
                if (initialTarget != null && initialTarget.gameObject.activeSelf) // Final check before applying damage
                {
                    rabbitBehaviour.InstantDeath();
                }
            }
        }

        yield return new WaitForSeconds(2f); // Adjust the total duration of the attack as needed

        target = null;

        navMeshAgent.isStopped = false;
        attacking = false;
        lastAttackTime = Time.time; // Update lastAttackTime to current time

        StartCoroutine(ResetAttackCooldown());
    }

    private void ResetAttackState()
    {
        target = null;
        navMeshAgent.isStopped = false;
        attacking = false;
        lastAttackTime = Time.time; // Update lastAttackTime to current time
        StartCoroutine(ResetAttackCooldown());
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackCooldownActive = false;
    }
}
