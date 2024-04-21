using UnityEngine;

public class LeafPoint : MonoBehaviour
{
    public delegate void LeafPointTriggerEnter(GameObject leafPoint);
    public static event LeafPointTriggerEnter OnLeafPointTriggerEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Parrot"))
        {
            OnLeafPointTriggerEnter?.Invoke(gameObject);
        }
    }
}

