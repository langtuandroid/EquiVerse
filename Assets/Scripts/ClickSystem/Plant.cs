using DG.Tweening;
using Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {
    private bool canBeConsumed = true;

    void Start() {
        EntityManager.Get().AddPlant(gameObject);
    }

    void OnDestroy() {
        EntityManager.Get().RemovePlant(gameObject);
    }

    public bool CanBeConsumed() {
        return canBeConsumed;
    }

    public void Consume() {
        canBeConsumed = false;
        gameObject.transform.DOScale(0, 0.6f).SetEase(Ease.OutBack);
        Destroy(gameObject, 0.6f);
        FindObjectOfType<PlantSpawner>().RemovePlant();
    }
}
