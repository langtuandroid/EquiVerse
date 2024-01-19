using DG.Tweening;
using Managers;
using Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Raycaster : MonoBehaviour {
    PlantSpawner plantSpawner;

    private void Start() {
        plantSpawner = FindObjectOfType<PlantSpawner>(true);
    }

    void Update() {
        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
                if (IsClickable(hit.collider.gameObject, hit.point)) {
                    // Handle the case when a collectable object is clicked (e.g., play a sound or show a message)
                } else { //We didn't click on an objects. Maybe on the ground?
                    plantSpawner.ClickOnGround(hit.point);
                }
            }
        }
    }

    private bool IsClickable(GameObject go, Vector3 point) {
        Clickable clickable = go.GetComponent<Clickable>();
        if (clickable) {
            clickable.OnClick(point);
            return true;
        }
        return false;
    }
}
