using DG.Tweening;
using Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {
    GameObject huntedBy = null;

    void Start() {
        EntityManager.Get().AddPlant(gameObject);
    }

    void OnDestroy() {
        EntityManager.Get().RemovePlant(gameObject);
    }

    public bool CanBeHunted(GameObject huntingObject) {
        return huntedBy == huntingObject || huntedBy == null;
    }

    public void SetHuntedBy(GameObject huntingObject) { 
        huntedBy = huntingObject;
    }

    public void Consume() {
        gameObject.transform.DOScale(0, 0.6f).SetEase(Ease.OutBack);
        Destroy(gameObject, 0.6f);
        FindObjectOfType<PlantSpawner>().RemovePlant();
    }
}
