using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float plantHealth = 100f; // Health of the plant

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is a rabbit
        RabbitBehaviour rabbit = other.GetComponent<RabbitBehaviour>();
        if (rabbit != null)
        {
            // Reduce the plant's health
            plantHealth -= rabbit.GetEatingPower();

            // Check if the plant's health is depleted
            if (plantHealth <= 0)
            {
                // Destroy the plant
                Destroy(gameObject);
            }
        }
    }
}
