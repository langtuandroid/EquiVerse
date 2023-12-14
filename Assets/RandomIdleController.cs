using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleController : MonoBehaviour
{
    private Animator animator;

    public GameObject eggPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("TriggerSecondIdle", Random.Range(5f, 15f));
        Invoke("EggDropTrigger", Random.Range(15f, 45f));
    }

    void TriggerSecondIdle()
    {
        animator.SetTrigger("SecondIdleTrigger");
        Invoke("TriggerSecondIdle", Random.Range(5f, 15f));
    }

    void EggDropTrigger()
    {
        animator.SetTrigger("EggDropTrigger");
        Invoke("EggDropTrigger", Random.Range(15f, 45f));
    }
}
