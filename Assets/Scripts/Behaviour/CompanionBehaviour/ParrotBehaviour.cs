using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParrotBehaviour : MonoBehaviour
{
    public Vector3 areaCenter;
    public Vector3 areaSize;
    public float moveSpeed = 5f;
    private GameObject targetLeafPoint;
    private Vector3 randomPosition;

    void Start()
    {
        randomPosition = GetRandomPosition();
    }

    void Update()
    {
        if (targetLeafPoint == null)
        {
            MoveTowardsPosition(randomPosition);
            if (Vector3.Distance(transform.position, randomPosition) < 0.1f)
            {
                randomPosition = GetRandomPosition();
            }
        }
        else
        {
            MoveTowardsPosition(targetLeafPoint.transform.position);
            if (Vector3.Distance(transform.position, targetLeafPoint.transform.position) < 0.1f)
            {
                targetLeafPoint = null;
            }
        }

        Collider[] leafPoints = Physics.OverlapBox(areaCenter, areaSize / 2);
        foreach (Collider leafPointCollider in leafPoints)
        {
            if (leafPointCollider.CompareTag("HighValue") || leafPointCollider.CompareTag("LowValue"))
            {
                targetLeafPoint = leafPointCollider.gameObject;
                break;
            }
        }
    }

    void MoveTowardsPosition(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360f);
        }

        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
    }

    Vector3 GetRandomPosition()
    {
        return areaCenter + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2),
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        print("collide!");
        if (other.gameObject.CompareTag("LowValue"))
        {
            other.gameObject.GetComponent<LowValueLeaf>().CollectLeafPoint();
        }else if (other.gameObject.CompareTag("HighValue"))
        {
            other.gameObject.GetComponent<HighValueLeaf>().CollectLeafPoint();
        }
    }
}
