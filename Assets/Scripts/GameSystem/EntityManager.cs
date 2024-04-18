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
    private List<GameObject> foxes = new List<GameObject>();
    private List<GameObject> foods = new List<GameObject>();

    public void AddRabbit(GameObject rabbit) {
        rabbits.Add(rabbit);
    }

    public void AddFox(GameObject fox)
    {
        foxes.Add(fox);
    }

    public void AddFood(GameObject food) {
        foods.Add(food);
    }

    public void RemoveRabbit(GameObject rabbit) {
        if (rabbits.Contains(rabbit)) {
            rabbits.Remove(rabbit);
        }
    }
    
    public void RemoveFox(GameObject fox) {
        if (foxes.Contains(fox)) {
            foxes.Remove(fox);
        }
    }

    public void RemoveFood(GameObject food) {
        if (foods.Contains(food)) {
            foods.Remove(food);
        }
    }

    public List<GameObject> GetRabbits() {
        return rabbits;
    }
    
    public List<GameObject> GetFoxes() {
        return foxes;
    }

    public List<GameObject> GetFoods() {
        return foods;
    }
}
