using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ConjurerMinionAttackBehaviour : MonoBehaviour
{
    public ParticleSystem explosionParticles;
    public ParticleSystem idleParticles;
    public NavMeshAgent agent;
    public GameObject minionModel;

    private float detectionRadius = 1.5f;
    private bool exploded;
    private float cooldownTillAttack = 5f;

    private void FixedUpdate()
    {
        if (cooldownTillAttack > 0) cooldownTillAttack -= Time.fixedDeltaTime;

        if (cooldownTillAttack <= 0)
        {
            SetTarget();
        }
    }

    private void SetTarget()
    {
        GameObject closestRabbit = null;
        float closestDistance = detectionRadius;

        foreach (var rabbit in EntityManager.Get().GetRabbits())
        {
            float distance = Vector3.Distance(transform.position, rabbit.transform.position);
            if (distance <= closestDistance)
            {
                closestRabbit = rabbit;
                closestDistance = distance;
            }
        }

        if (closestRabbit != null)
        {
            agent.SetDestination(closestRabbit.transform.position);
        }
    }

    private void OnTriggerEnter(Collider animal)
    {
        if (exploded || cooldownTillAttack > 0 || !animal.CompareTag("Animal")) return;

        var rabbitBehaviour = animal.GetComponent<MalbersRabbitBehaviour>();
        if (rabbitBehaviour == null) return;

        exploded = true;
        minionModel.SetActive(false);
        rabbitBehaviour.InstantDeath(false);

        explosionParticles?.Play();
        idleParticles?.Stop();

        DOVirtual.DelayedCall(explosionParticles.main.duration, () => Destroy(gameObject));
    }
}