using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class LeafPointsSpawner : MonoBehaviour
    {
        private GameManager gameManager;
        public GrowthManager growthManager;
        public GameObject lowValueleafPointPrefab, highValueleafPointPrefab;
        private float spawnTimer = 0f;
        private float timeBetweenLeafSpawn;
        public Vector2 timeBetweenLeafSpawnRange = new Vector2(10f, 18f);
        private float desiredHeight = 5f;
        public static float duration = 10f;

        public bool spawnLeafPoints = true;

        private void Start()
        {
            TutorialManager.pickUpLeafpointStepCompleted = false;
            timeBetweenLeafSpawn = Random.Range(timeBetweenLeafSpawnRange.x, timeBetweenLeafSpawnRange.y);

            gameManager = FindObjectOfType<GameManager>();
        }

        private void FixedUpdate()
        {
            if (!EnemySpawner.enemyDanger)
            {
                if (spawnLeafPoints && growthManager.isAdolescent)
                {
                    spawnTimer += Time.fixedDeltaTime;
                    if (spawnTimer >= timeBetweenLeafSpawn)
                    {
                        Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
                        GameObject newLeaf;
                        newLeaf = Instantiate(lowValueleafPointPrefab, spawnPosition, Quaternion.identity);
                        newLeaf.transform.DOMoveY(desiredHeight, duration).SetEase(Ease.OutCubic)
                            .OnComplete(() => FadeAndDestroy(newLeaf));
                        spawnTimer = 0f;

                        if (gameManager.level1 && !TutorialManager.pickUpLeafpointStepCompleted)
                        {
                            TutorialManager.GoToNextStep();
                            TutorialManager.pickUpLeafpointStepCompleted = true;
                        }

                        timeBetweenLeafSpawn = Random.Range(timeBetweenLeafSpawnRange.x, timeBetweenLeafSpawnRange.y);
                    }
                }

                if (spawnLeafPoints && growthManager.isAdult)
                {
                    spawnTimer += Time.fixedDeltaTime;
                    if (spawnTimer >= timeBetweenLeafSpawn)
                    {
                        Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
                        GameObject newLeaf;
                        newLeaf = Instantiate(highValueleafPointPrefab, spawnPosition, Quaternion.identity);
                        newLeaf.transform.DOMoveY(desiredHeight, duration).SetEase(Ease.OutCubic)
                            .OnComplete(() => FadeAndDestroy(newLeaf));
                        spawnTimer = 0f;
                        timeBetweenLeafSpawn = Random.Range(timeBetweenLeafSpawnRange.x, timeBetweenLeafSpawnRange.y);
                    }
                }
            }
        }

        private void FadeAndDestroy(GameObject obj)
        {
            Material material = obj.GetComponent<Renderer>().material;
            material.DOFade(0f, 1f).OnComplete(() => Destroy(obj));
        }
    }
}
