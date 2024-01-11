using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public class EnemyHealth : Clickable
{
    public int enemyHealth;
    public override void OnClick()
    {
        if (enemyHealth > GunBehaviour.gunDamage)
        {
            enemyHealth -= GunBehaviour.gunDamage;
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        transform.DOScale(0, 1f).SetEase(Ease.OutExpo);
        Destroy(gameObject, 1f);
    }
}
