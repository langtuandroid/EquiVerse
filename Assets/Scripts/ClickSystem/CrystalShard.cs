using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Managers;

public class CrystalShard : Clickable
{
    public GameObject particleEffectPrefab;

    public override void OnClick() {
        ECManager ecManager = FindObjectOfType<ECManager>();

        if (ecManager != null) {
            ecManager.AddGooseEggPoints();
            StartCoroutine(DestroyObjectWithEffect(gameObject));
        }
    }
    
    
    void Start()
    {
        transform.DOMoveY(2f, 1f).SetEase(Ease.OutExpo).OnComplete((() =>
        {
            Vector3 initialPosition = transform.position;
            transform.DOMoveY(initialPosition.y + 0.3f, 2.0f)
                .SetEase(Ease.InOutQuad) // Adjust the easing function as needed
                .SetLoops(-1, LoopType.Yoyo); // Set it to loop indefinitely 
        }));
    }
    
    private IEnumerator DestroyObjectWithEffect(GameObject obj) {
        Destroy(obj);
        GameObject particleEffect = Instantiate(particleEffectPrefab, obj.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(particleEffect);
    }
}
