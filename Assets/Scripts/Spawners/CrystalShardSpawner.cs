using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class CrystalShardSpawner : MonoBehaviour
    {
        public GameObject crystalShardPrefab;
        private float spawnTimer = 0f;
        private float timeBetweenCrystalShardSpawn;
        public Vector2 timeBetweenCrystalShardSpawnRange = new Vector2(10f, 18f);
        private float desiredHeight = 5f;
        private float duration = 5f;

        public static bool spawnCrystalShardPoints = true;

        private void Start()
        {
            timeBetweenCrystalShardSpawn = Random.Range(timeBetweenCrystalShardSpawnRange.x, timeBetweenCrystalShardSpawnRange.y);
        }

        private void FixedUpdate()
        {
            if (spawnCrystalShardPoints)
            {
                spawnTimer += Time.fixedDeltaTime;
                if (spawnTimer >= timeBetweenCrystalShardSpawn)
                {
                    Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
                    GameObject newCrystalShard;
                    newCrystalShard = Instantiate(crystalShardPrefab, spawnPosition, Quaternion.identity);
                    newCrystalShard.transform.DOMoveY(desiredHeight, duration).SetEase(Ease.OutCubic)
                        .OnComplete(() => FadeAndDestroy(newCrystalShard));

                    spawnTimer = 0f;
                    timeBetweenCrystalShardSpawn = Random.Range(timeBetweenCrystalShardSpawnRange.x, timeBetweenCrystalShardSpawnRange.y);
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
