using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseEgg : Clickable {
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        ECManager ecManager = FindObjectOfType<ECManager>();

        if (ecManager != null) {
            ecManager.AddGooseEggPoints();
            StartCoroutine(DestroyObjectWithEffect(gameObject));
        }
    }

    private IEnumerator DestroyObjectWithEffect(GameObject obj) {
        Destroy(obj);
        GameObject particleEffect = Instantiate(particleEffectPrefab, obj.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(particleEffect);
    }
}
