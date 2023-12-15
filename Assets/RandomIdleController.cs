using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleController : MonoBehaviour
{
    private Animator animator;

    public GameObject eggPrefab;

    public float secondIdleTriggerMinWait = 5f;
    public float secondIdleTriggerMaxWait = 15f;

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
        Invoke("EggDropTrigger", Random.Range(15f, 45f));
    }

    void TriggerSecondIdle()
    {
        animator.SetTrigger("SecondIdleTrigger");
        Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
    }

    void EggDropTrigger()
    {
        animator.SetTrigger("EggDropTrigger");
        Invoke("EggDropTrigger", Random.Range(15f, 45f));
    }
}
