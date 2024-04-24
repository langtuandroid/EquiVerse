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
    
    public float soundTriggerMinWait = 5f;
    public float soundTriggerMaxWait = 15f;

    void Start()
    {
        randomPosition = GetRandomPosition();
        InvokeRepeating("TriggerSound", 0, Random.Range(soundTriggerMinWait, soundTriggerMaxWait));
    }

    void Update()
    {
        GameObject closestLeafPoint = null;
        float closestDistance = Mathf.Infinity;

        Collider[] leafPoints = Physics.OverlapBox(areaCenter, areaSize / 2);
        foreach (Collider leafPointCollider in leafPoints)
        {
            if (leafPointCollider.CompareTag("HighValue") || leafPointCollider.CompareTag("LowValue"))
            {
                float distance = Vector3.Distance(transform.position, leafPointCollider.transform.position);
        
                if (distance < closestDistance)
                {
                    closestLeafPoint = leafPointCollider.gameObject;
                    closestDistance = distance;
                }
            }
        }

        if (closestLeafPoint != null)
        {
            MoveTowardsPosition(closestLeafPoint.transform.position);
        }
        else
        {
            MoveTowardsPosition(randomPosition);

            if (Vector3.Distance(transform.position, randomPosition) < 0.1f)
            {
                randomPosition = GetRandomPosition();
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/PabloParrot/PabloPickup");
        if (other.gameObject.CompareTag("LowValue"))
        {
            other.gameObject.GetComponent<LowValueLeaf>().CollectLeafPoint();
        }else if (other.gameObject.CompareTag("HighValue"))
        {
            other.gameObject.GetComponent<HighValueLeaf>().CollectLeafPoint();
        }
    }

    void TriggerSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/PabloParrot/PabloIdle");
    }
}
