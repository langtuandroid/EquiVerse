using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemyFXController : MonoBehaviour
{
    public static bool attacking = false;
    public ParticleSystem leftFootImpact, rightFootImpact, attackOnHit;
    
    public void RightImpactEvent()
    {
        rightFootImpact.Play();
    }

    public void LeftImpactEvent()
    {
        leftFootImpact.Play();
    }

    public void AttackOnHit()
    {
        
    }
    
    public void OnAttackAnimationComplete()
    {
        attacking = false;
    }
}
