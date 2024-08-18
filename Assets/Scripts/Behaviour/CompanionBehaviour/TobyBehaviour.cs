using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using FMODUnity;
using Unity.AI.Navigation;

public class TobyBehaviour : MonoBehaviour
{
    public GameObject throwableHayPrefab, throwableCarrotPrefab;
    public static float minTimeTillNextThrow = 8, maxTimeTillNextThrow = 10;
    public float throwDuration;
    public float arcHeight;
    public float navMeshSampleDistance;
    public float minThrowDistance;
    public float throwAngle;
    public EventReference throwSound, landSound;

    private NavMeshSurface navMeshSurface;
    private float timer;
    private Animator animator;
    private GameObject throwableFoodPrefab;

    public static bool foodQualityUpgrade = false;

    void Start()
    {
        if (!foodQualityUpgrade)
        {
            throwableFoodPrefab = throwableHayPrefab;
        }
        else
        {
            throwableFoodPrefab = throwableCarrotPrefab;
        }

        animator = GetComponent<Animator>();
        GameObject navMeshObject = GameObject.Find("PlantPlacement_NavMesh_Surface");
        if (navMeshObject != null)
        {
            navMeshSurface = navMeshObject.GetComponent<NavMeshSurface>();
        }
        timer = Random.Range(minTimeTillNextThrow, maxTimeTillNextThrow);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ThrowFood();
            timer = Random.Range(minTimeTillNextThrow, maxTimeTillNextThrow);
        }
    }

    private void ThrowFood()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = GetRandomPointOnNavMesh(startPos, navMeshSampleDistance);

        GameObject food = Instantiate(throwableFoodPrefab, startPos, Quaternion.identity);
        food.transform.localScale = Vector3.zero;
        food.transform.DOScale(0.5f, 0.75f).SetEase(Ease.OutElastic);
        animator.SetTrigger("ThrowFoodTrigger");
        RuntimeManager.PlayOneShot(throwSound);

        Food foodScript = food.GetComponent<Food>();
        if (foodScript != null)
        {
            foodScript.isSpawnedByExternal = true;
        }

        Vector3 midPos = (startPos + endPos) / 2;
        midPos.y += arcHeight;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(food.transform.DOPath(new Vector3[] { startPos, midPos, endPos }, throwDuration, PathType.CatmullRom)
            .SetEase(Ease.OutBack));
        sequence.OnComplete(() => 
        {
        });
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float maxDistance)
    {
        Vector3 randomDirection;
        NavMeshHit hit;

        while (true)
        {
            // Generate a random point within a cone in the forward direction
            randomDirection = RandomPointInCone(center, transform.forward, throwAngle, maxDistance);

            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas))
            {
                if (Vector3.Distance(center, hit.position) >= minThrowDistance)
                {
                    return hit.position;
                }
            }
        }
    }

    private Vector3 RandomPointInCone(Vector3 origin, Vector3 direction, float angle, float distance)
    {
        float halfAngle = angle / 2.0f;
        float randomAngle = Random.Range(-halfAngle, halfAngle);
        float randomDistance = Random.Range(minThrowDistance, distance);
        
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        Vector3 directionInCone = rotation * direction;

        return origin + directionInCone.normalized * randomDistance;
    }
}
