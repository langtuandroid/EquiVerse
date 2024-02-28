using Spawners;
using UnityEngine;

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
                } else {
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
