using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class CrystalShardFox : Clickable
{
    public GameObject particleEffectPrefab;

    public override void OnClick(Vector3 point) {
        LeafPointManager leafPointManager = FindObjectOfType<LeafPointManager>();

        if (leafPointManager != null) {
            leafPointManager.AddCrystalShardPoints();
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerActions/CollectCrystal");
            Destroy(gameObject);
            GameObject particleEffect = Instantiate(particleEffectPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(particleEffect, 1.5f);
        }
    }
}
