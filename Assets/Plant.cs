using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {
    void Start() {
        EntityManager.AddPlant(gameObject);
    }

    void OnDestroy() {
        EntityManager.RemovePlant(gameObject);
    }
}
