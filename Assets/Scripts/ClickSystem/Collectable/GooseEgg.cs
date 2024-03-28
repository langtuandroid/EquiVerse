using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseEgg : Clickable {
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        LeafPointManager leafPointManager = FindObjectOfType<LeafPointManager>();

        if (leafPointManager != null) {
            leafPointManager.AddGooseEggPoints();
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/CollectEgg");
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
