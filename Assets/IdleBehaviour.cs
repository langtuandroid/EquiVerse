using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private float timeUntilOtherAnimation;

    [SerializeField] private int numberOfAnimations;

    private bool isBored;
    private float idleTime;
    private int idleAnimation;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isBored)
        {
            idleTime += Time.deltaTime;
            if (idleTime > timeUntilOtherAnimation && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isBored = true;
                idleAnimation = Random.Range(1, numberOfAnimations + 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetIdle();
        }
        animator.SetFloat("IdleAnimation", idleAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        isBored = false;
        idleTime = 0;
        idleAnimation = 0;
    }
}
