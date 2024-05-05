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
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        bool isInPursuit = false; // Flag to check if currently in pursuit

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(enemyTag))
            {
                Vector3 targetPosition = col.transform.position;
                // Add a y-axis offset to the target position
                targetPosition.y += yOffset;

                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * beeSpeed * Time.deltaTime;
                isInPursuit = true; // Set flag to true if in pursuit
            }
        }

        // Check if state has changed, then update and fade accordingly
        if (!wasInPursuit && isInPursuit)
        {
            soundController.FadeAudioParameter("BetsyZooming", "InPursuit", 1f, 0.4f);
            hasTarget = true;
            isReturning = false;
        }
        else if (wasInPursuit && !isInPursuit)
        {
            soundController.FadeAudioParameter("BetsyZooming", "InPursuit", 0f, 0.4f);
            isReturning = true;
        }

        wasInPursuit = isInPursuit; // Update the previous state

        if (!hasTarget && isReturning)
        {
            ReturnToOriginalPosition();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag) && Time.time >= nextAttackTime)
        {
            print("hit");
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
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
    }
}
