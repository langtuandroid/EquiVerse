using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemyFXController : MonoBehaviour
{
    public bool attacking = false;
    public ParticleSystem leftFootImpact, rightFootImpact;
    public ParticleSystem summonSpikesParticles;
    
    public void RightImpactEvent()
    {
        rightFootImpact.Play();
    }

    public void LeftImpactEvent()
    {
        leftFootImpact.Play();
    }

    public void SummonSpikesEvent()
    {
        summonSpikesParticles.Play();
    }
}
