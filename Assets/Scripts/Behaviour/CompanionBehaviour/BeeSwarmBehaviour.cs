using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwarmBehaviour : MonoBehaviour
{
    public ParticleSystem hitParticles;
    private string enemyTag = "Enemy";
    private float detectionRadius = 10f;
    private float returnSpeed = 2f;
    private float beeSpeed = 5f;
    private float damage = 10f;
    private float attackCooldown = 0.75f;
    private float nextAttackTime;

    private Vector3 originalPosition;
    private bool hasTarget = false;
    private float yOffset = 1.0f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(enemyTag))
            {
                Vector3 targetPosition = col.transform.position;
                // Add a y-axis offset to the target position
                targetPosition.y += yOffset;
        
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * beeSpeed * Time.deltaTime;
                hasTarget = true;
            }
        }

        if (!hasTarget)
        {
            ReturnToOriginalPosition();
        }
        else
        {
            hasTarget = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            if (Time.time >= nextAttackTime)
            {
                print("hit");
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.enemyHealth -= 20;
                    hitParticles.Play();

                    if (enemyHealth.enemyHealth <= 0)
                    {
                        enemyHealth.Die();
                    }
                }

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
    }
}
