using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is a rabbit
        RabbitBehaviour rabbit = other.GetComponent<RabbitBehaviour>();
        if (rabbit != null && rabbit.isHungry)
        {
            rabbit.ReduceHunger(30f);
            Destroy(gameObject);
        }
    }
}
