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
    
    public override void OnClick(Vector3 point)
    {
        GunUpgrade currentGunUpgrade = UpgradeManager.GetInstance().GetCurrentGunUpgrade();
        if (enemyHealth > currentGunUpgrade.gunDamage)
        {
            enemyHealth -= currentGunUpgrade.gunDamage;
            ParticleSystem p = Instantiate(currentGunUpgrade.gunParticles,point, Quaternion.identity);
            p.Play();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        transform.DOScale(0, 0.2f).SetEase(Ease.OutQuint);
        Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
        Instantiate(reward, gameObject.transform.position + new Vector3(0,0.3f,0) , Quaternion.identity);
        
        Destroy(gameObject, 1f);
        Destroy(deathParticles, 5f);
    }
}
