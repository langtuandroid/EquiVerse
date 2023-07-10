using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public void PlantSelected()
    {
        ObjectSpawner.rabbitSelected = false;
        ObjectSpawner.grassSelected = !ObjectSpawner.grassSelected;
    }
    public void RabbitSelected()
    {
        ObjectSpawner.grassSelected = false;
        ObjectSpawner.rabbitSelected = !ObjectSpawner.rabbitSelected;
    }
}
