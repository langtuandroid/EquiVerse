using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemyFXController : MonoBehaviour
{
    public ParticleSystem leftFootImpact, rightFootImpact;
    public FMODUnity.EventReference footGroundImpactSound; 
    
    public void RightImpactEvent()
    {
        rightFootImpact.Play();
        FMODUnity.RuntimeManager.PlayOneShot(footGroundImpactSound);
    }

    public void LeftImpactEvent()
    {
        leftFootImpact.Play();
        FMODUnity.RuntimeManager.PlayOneShot(footGroundImpactSound);
    }
}
