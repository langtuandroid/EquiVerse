using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {
    private static EntityManager _instance;

    public void Start() {
        _instance = this;
    }

    public static EntityManager Get() {
        return _instance;
    }

    private List<GameObject> rabbits = new List<GameObject>();
    private List<GameObject> plants = new List<GameObject>();

    public void AddRabbit(GameObject rabbit) {
        rabbits.Add(rabbit);
    }

    public void AddPlant(GameObject plant) {
        plants.Add(plant);
    }

    public void RemoveRabbit(GameObject rabbit) {
        if (rabbits.Contains(rabbit)) {
            rabbits.Remove(rabbit);
        }
    }

    public void RemovePlant(GameObject plant) {
        if (plants.Contains(plant)) {
            plants.Remove(plant);
        }
    }

    public List<GameObject> GetRabbits() {
        return rabbits;
    }

    public List<GameObject> GetPlants() {
        return plants;
    }
}
