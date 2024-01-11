using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityManager {
    private static List<GameObject> rabbits = new List<GameObject>();
    private static List<GameObject> plants = new List<GameObject>();

    public static void AddRabbit(GameObject rabbit) {
        rabbits.Add(rabbit);
    }

    public static void AddPlant(GameObject plant) {
        plants.Add(plant);
    }

    public static void RemoveRabbit(GameObject rabbit) {
        if (rabbits.Contains(rabbit)) {
            rabbits.Remove(rabbit);
        }
    }

    public static void RemovePlant(GameObject plant) {
        if (plants.Contains(plant)) {
            plants.Remove(plant);
        }
    }

    public static List<GameObject> GetRabbits() {
        return rabbits;
    }

    public static List<GameObject> GetPlants() {
        return plants;
    }
}
