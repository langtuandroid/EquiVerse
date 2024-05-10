using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class BeeSwarmBehaviour : MonoBehaviour
{
    public World1LevelSoundController soundController;
    public ParticleSystem hitParticles;
    public FMODUnity.EventReference betsyAttackSound;
    private string enemyTag = "Enemy";
    private float detectionRadius = 10f;
    private float returnSpeed = 2f;
    private float beeSpeed = 5f;
    private float damage = 10f;
    private float attackCooldown = 0.75f;
    private float nextAttackTime;

    private Vector3 originalPosition;
    private List<Transform> enemiesInRange = new List<Transform>();
    private bool hasTarget = false;
    private bool isReturning = false;
    private bool wasInPursuit = false; // Track if previously in pursuit
    private float yOffset = 1.0f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        CheckForEnemies();

        if (enemiesInRange.Count > 0)
        {
            PursueEnemies();
        }
        else
        {
            isReturning = true;
            ReturnToOriginalPosition();
        }
    }

    private void CheckForEnemies()
    {
        enemiesInRange.Clear(); // Clear the list before checking again

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(enemyTag))
            {
                enemiesInRange.Add(col.transform);
            }
        }
    }

    private void PursueEnemies()
    {
        foreach (Transform enemy in enemiesInRange)
        {
            Vector3 targetPosition = enemy.position;
            // Add a y-axis offset to the target position
            targetPosition.y += yOffset;

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * beeSpeed * Time.deltaTime;
            hasTarget = true; // Indicate that the bee has a target
        }

        // Update audio and pursuit status
        if (!wasInPursuit)
        {
            soundController.FadeAudioParameter("BetsyZooming", "InPursuit", 1f, 0.4f);
            wasInPursuit = true;
            isReturning = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag) && Time.time >= nextAttackTime)
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.enemyHealth -= 20;
                hitParticles.Play();
                FMODUnity.RuntimeManager.PlayOneShot(betsyAttackSound);

                if (enemyHealth.enemyHealth <= 0)
                {
                    enemyHealth.Die();
                    hasTarget = false;
                    isReturning = true;
                }
            }

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void ReturnToOriginalPosition()
    {
        // Move towards the original position
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);

        // If reached original position, reset pursuit status
        if (transform.position == originalPosition)
        {
            if (wasInPursuit)
            {
                soundController.FadeAudioParameter("BetsyZooming", "InPursuit", 0f, 0.4f);
                wasInPursuit = false;
            }
        }
    }
}
