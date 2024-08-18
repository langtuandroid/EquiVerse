using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PabloBehaviour : MonoBehaviour
{
    public Vector3 areaCenter;
    public Vector3 areaSize;
    public static float moveSpeed = 1.5f;
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
        GameObject closestCollectable = null;
        float closestDistance = Mathf.Infinity;

        Collider[] collectables = Physics.OverlapBox(areaCenter, areaSize / 2);
        foreach (Collider collectableCollider in collectables)
        {
            if (collectableCollider.CompareTag("HighValue") || collectableCollider.CompareTag("LowValue") || collectableCollider.CompareTag("CrystalShard"))
            {
                float distance = Vector3.Distance(transform.position, collectableCollider.transform.position);
        
                if (distance < closestDistance)
                {
                    closestCollectable = collectableCollider.gameObject;
                    closestDistance = distance;
                }
            }
        }

        if (closestCollectable != null)
        {
            MoveTowardsPosition(closestCollectable.transform.position);
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
        }else if (other.gameObject.CompareTag("CrystalShard"))
        {
            other.gameObject.GetComponent<CrystalShardFox>().CollectFoxCrystalShard();
        }
    }

    void TriggerSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/PabloParrot/PabloIdle");
    }
}
