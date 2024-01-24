using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemyFXController : MonoBehaviour
{
    public static bool attacking = false;
    public ParticleSystem leftFootImpact, rightFootImpact;
    
    public void RightImpactEvent()
    {
        rightFootImpact.Play();
    }

    public void LeftImpactEvent()
    {
        leftFootImpact.Play();
    }
    
    public void OnAttackAnimationComplete()
    {
        print("test");
        attacking = false;
    }
}
