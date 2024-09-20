using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GrimwoodConjurerAttackBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject minionPrefab;
    public Animator animator;
    public ParticleSystem chargingParticles;
    public ParticleSystem releaseParticles;

    private int amountofMinionsSpawned = 2;
    private float spawnRadius = 3f;

    private bool attacking = false;
    private bool attackCooldown = false;

    private void Start()
    {
        InvokeRepeating("StartAttackSequence", 3f, UnityEngine.Random.Range(12f, 15f));
    }

    private void StartAttackSequence()
    {
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        attacking = true;
        attackCooldown = true;
        agent.isStopped = true;
        animator.SetTrigger("AttackTrigger");
        chargingParticles.Play();

        yield return new WaitForSeconds(1.5f);
        
        chargingParticles.Stop();
        releaseParticles.Play();

        Vector3 conjurerPosition = transform.position;

        for (int i = 0; i < amountofMinionsSpawned; i++)
        {
            float angle = i * Mathf.PI * 2 / amountofMinionsSpawned;
            Vector3 spawnOffset = new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);
            Vector3 spawnPosition = conjurerPosition + spawnOffset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, spawnRadius, new NavMeshQueryFilter { agentTypeID = NavMesh.GetSettingsByIndex(2).agentTypeID, areaMask = NavMesh.AllAreas }))
            {
                Instantiate(minionPrefab, hit.position, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(1.5f);

        agent.isStopped = false;
        attacking = false;

        yield return new WaitForSeconds(2f);
        attackCooldown = false;
    }
}
