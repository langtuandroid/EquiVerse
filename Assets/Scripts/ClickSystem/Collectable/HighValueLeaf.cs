using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class HighValueLeaf : Clickable
{
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        CollectLeafPoint();
    }

    public void CollectLeafPoint()
    {
        LeafPointManager leafPointManager = FindObjectOfType<LeafPointManager>();

        if (leafPointManager != null) {
            leafPointManager.AddHighValuePoints();
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/CollectLeafPoint");
            Destroy(gameObject);
            GameObject particleEffect = Instantiate(particleEffectPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(particleEffect, 1.5f);
        }
    }
}
