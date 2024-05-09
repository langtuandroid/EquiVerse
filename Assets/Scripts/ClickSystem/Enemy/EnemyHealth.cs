using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public class EnemyHealth : Clickable
{
    public int enemyHealth;
    public GameObject reward;
    public GameObject deathParticles;
    public FMODUnity.EventReference deathSound;
    private Tween punchTween;

    public override void OnClick(Vector3 point)
    {
        GunUpgrade currentGunUpgrade = GunUpgradeManager.GetInstance().GetCurrentGunUpgrade();
        if (enemyHealth > currentGunUpgrade.gunDamage)
        {
            if (punchTween != null && punchTween.IsActive())
            {
                punchTween.Complete(); // Complete the previous tween
                punchTween = null; // Reset the tween reference
            }

            enemyHealth -= currentGunUpgrade.gunDamage;
        
            punchTween = transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 8, 1);

            ParticleSystem particleSystem = Instantiate(currentGunUpgrade.gunParticles, point, Quaternion.identity);
            particleSystem.Play();
            GameObject gunParticleInstance = particleSystem.gameObject;
            Destroy(gunParticleInstance, 1f);

            FMODUnity.RuntimeManager.PlayOneShot(currentGunUpgrade.gunImpactSoundEventPath);
            
            CameraShake.Instance.ShakeCamera(2f, 0.15f);
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        transform.DOScale(0, 0.2f).SetEase(Ease.OutQuint);
        FMODUnity.RuntimeManager.PlayOneShot(deathSound);
        GameObject particlesInstance = Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
        Instantiate(reward, gameObject.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.RemoveEnemy(gameObject);
    
        Destroy(particlesInstance, 5f);
    }
}
