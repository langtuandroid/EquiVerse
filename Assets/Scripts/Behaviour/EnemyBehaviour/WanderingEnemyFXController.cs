using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemyFXController : MonoBehaviour
{
    public ParticleSystem leftFootImpact, rightFootImpact;
    
    public void RightImpactEvent()
    {
        rightFootImpact.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/FootGroundImpact");
    }

    public void LeftImpactEvent()
    {
        leftFootImpact.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/FootGroundImpact");
    }
}
