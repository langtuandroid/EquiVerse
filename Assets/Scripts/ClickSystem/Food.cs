using DG.Tweening;
using Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int foodGrowthValue;
    private bool canBeConsumed = true;

    void Start() {
        EntityManager.Get().AddFood(gameObject);
    }

    void OnDestroy() {
        EntityManager.Get().RemoveFood(gameObject);
    }

    public bool CanBeConsumed() {
        return canBeConsumed;
    }

    public void Consume() {
        canBeConsumed = false;
        gameObject.transform.DOScale(0, 0.6f).SetEase(Ease.OutBack);
        Destroy(gameObject, 0.6f);
        FindObjectOfType<FoodSpawner>().RemovePlant();
    }
}
