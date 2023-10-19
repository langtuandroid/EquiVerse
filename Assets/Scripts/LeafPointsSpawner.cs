using UnityEngine;
using DG.Tweening;

public class LeafPointsSpawner : MonoBehaviour
{
    public GameObject leafPointPrefab;
    private float spawnTimer = 0f;
    private float timeBetweenLeafSpawn = 5f;
    private float desiredHeight = 5f; // Adjust this value for the desired height
    private float duration = 5f; // Adjust the move speed as needed

    private void FixedUpdate()
    {
        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer >= timeBetweenLeafSpawn)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0,0.5f,0); // Use the position of the object
            GameObject newLeaf = Instantiate(leafPointPrefab, spawnPosition, Quaternion.identity);

            // Use DoTween to move the object to the desired height
            newLeaf.transform.DOMoveY(desiredHeight, duration).SetEase(Ease.OutSine).OnComplete(() => FadeAndDestroy(newLeaf));

            spawnTimer = 0f;
        }
    }

    private void FadeAndDestroy(GameObject obj)
    {
        Material material = obj.GetComponent<Renderer>().material;
        material.DOFade(0f, duration).OnComplete(() => Destroy(obj));
    }
}
