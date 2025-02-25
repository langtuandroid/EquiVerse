using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Managers;

public class CrystalShardEnemy : Clickable
{
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        LeafPointManager leafPointManager = FindObjectOfType<LeafPointManager>();

        if (leafPointManager != null) {
            leafPointManager.AddCrystalShardPoints();
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/CollectCrystal");
            Destroy(gameObject);
            GameObject particleEffect = Instantiate(particleEffectPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(particleEffect, 1f);
        }
    }
    
    void Start()
    {
        transform.localScale = Vector3.zero;
        
        transform.DOMoveY(2f, 2f).SetEase(Ease.InSine).OnComplete((() =>
        {
            Vector3 initialPosition = transform.position;
            transform.DOMoveY(initialPosition.y + 0.3f, 2.0f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }));
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }
}
