using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Managers;

public class CrystalShard : Clickable
{
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        ECManager ecManager = FindObjectOfType<ECManager>();

        if (ecManager != null) {
            ecManager.AddCrystalShardPoints();
            StartCoroutine(DestroyObjectWithEffect(gameObject));
        }
    }
    
    
    void Start()
    {
        transform.localScale = Vector3.zero;
        
        transform.DOMoveY(2f, 2f).SetEase(Ease.InSine).OnComplete((() =>
        {
            Vector3 initialPosition = transform.position;
            transform.DOMoveY(initialPosition.y + 0.3f, 2.0f)
                .SetEase(Ease.InOutQuad) // Adjust the easing function as needed
                .SetLoops(-1, LoopType.Yoyo); // Set it to loop indefinitely 
        }));
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }
    
    private IEnumerator DestroyObjectWithEffect(GameObject obj) {
        Destroy(obj);
        GameObject particleEffect = Instantiate(particleEffectPrefab, obj.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(particleEffect);
    }
}
