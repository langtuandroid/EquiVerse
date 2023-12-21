using System.Collections;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviour
{
    public class WanderScript : MonoBehaviour
    {
        [SerializeField] private float wanderRadius;
        [SerializeField] private Vector2 wanderTimerRange = new Vector2(3f, 7f);
        [SerializeField] private Vector2 idleDurationRange = new Vector2(2f, 5f);
        [SerializeField] private Vector2 speedRange = new Vector2(0.5f, 1.0f);
        [SerializeField] private float hungerThreshold;
        [SerializeField] private float warningThreshold;
        [SerializeField] private float deathThreshold;
        [SerializeField] private Material hungryMaterial;
        [SerializeField] private ParticleSystem landingParticleSystem;

        private SkinnedMeshRenderer skinnedMeshRenderer;
        private NavMeshAgent agent;
        private Animator animator;
        private Material rabbitMaterial;

        private float wanderTimer;
        private float idleDuration;
        private float speed;
        private float stateTimer;
        private bool isIdling = false;
        private bool isHungry = false;
        private bool inWarningState = false;
        private float currentHunger = 0f;

        private const float LEAF_SPAWN_DISTANCE_THRESHOLD = 0.2f;
        private const float SMOOTHING_FACTOR = 5f;
        private const float ROTATION_SPEED = 360f;

        private void Start()
        {
            InitializeComponents();
            SwitchToIdle();
        }

        private void FixedUpdate()
        {
            MaterialChanger();
            HandleHunger();

            if (isHungry)
                FindClosestPlant();
            else
                HandleWanderAndIdle();

            SmoothMovement();
            AlignRotation();
        }

        private void InitializeComponents()
        {
            agent = GetComponent<NavMeshAgent>();
            InitializeAgentParameters();

            animator = GetComponent<Animator>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            rabbitMaterial = skinnedMeshRenderer.material;
        }

        private void InitializeAgentParameters()
        {
            agent.angularSpeed = 120f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 0.2f;
        }

        private void HandleHunger()
        {
            currentHunger += 5f * Time.fixedDeltaTime;

            if (currentHunger >= deathThreshold)
                StartCoroutine(Die());

            if (currentHunger >= hungerThreshold)
            {
                isHungry = true;

                if (currentHunger >= warningThreshold)
                {
                    LeafPointsSpawner.spawnLeafPoints = false;
                    inWarningState = true;
                }
            }
        }

        private void HandleWanderAndIdle()
        {
            stateTimer -= Time.fixedDeltaTime;

            if (stateTimer <= 0f)
            {
                if (isIdling)
                    SwitchToWander();
                else
                    SwitchToIdle();
            }
        }

        private void SwitchToWander()
        {
            isIdling = false;
            animator.SetBool("isJumping", true);
            Wander();
        }

        private void SwitchToIdle()
        {
            isIdling = true;
            animator.SetBool("isJumping", false);
            Idle();
        }

        private void Wander()
        {
            SetWanderParameters();

            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
            agent.SetDestination(navHit.position);
        }

        private void SetWanderParameters()
        {
            wanderTimer = Random.Range(wanderTimerRange.x, wanderTimerRange.y);
            idleDuration = 0f;
            stateTimer = wanderTimer;

            SetRandomSpeed();
        }

        private void SetRandomSpeed()
        {
            speed = Random.Range(speedRange.x, speedRange.y);
            agent.speed = speed;
        }

        private void Idle()
        {
            agent.speed = 0f;
            idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
            stateTimer = idleDuration;
        }

        private void FindClosestPlant()
        {
            GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

            if (plants.Length == 0)
            {
                HandleNoPlantsFound();
                return;
            }

            Transform closestPlant = FindClosestPlantTransform(plants);

            if (closestPlant != null)
                HandleClosestPlant(closestPlant);
        }

        private void HandleNoPlantsFound()
        {
            animator.SetBool("isRunning", false);
            HandleWanderAndIdle();
        }

        private void HandleClosestPlant(Transform closestPlant)
        {
            if (Vector3.Distance(transform.position, closestPlant.position) > LEAF_SPAWN_DISTANCE_THRESHOLD)
            {
                MoveTowardsPlant(closestPlant);
            }
            else
            {
                EatPlant(closestPlant);
            }
        }

        private void MoveTowardsPlant(Transform closestPlant)
        {
            agent.speed = 1.3f;
            agent.SetDestination(closestPlant.position);
            animator.SetBool("isRunning", true);
        }

        private void EatPlant(Transform closestPlant)
        {
            animator.SetBool("isRunning", false);
            Destroy(closestPlant.gameObject);
            PlantSpawner.RemovePlant();
            currentHunger -= 100f;
            LeafPointsSpawner.spawnLeafPoints = true;
            isHungry = false;
            inWarningState = false;
            SetIdleAnimation();
        }

        private void SetIdleAnimation()
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            isIdling = true;
            HandleWanderAndIdle();
        }

        private Transform FindClosestPlantTransform(GameObject[] plants)
        {
            Transform closestPlant = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject plant in plants)
            {
                float distance = Vector3.Distance(transform.position, plant.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlant = plant.transform;
                }
            }

            return closestPlant;
        }

        private void MaterialChanger()
        {
            skinnedMeshRenderer.material = inWarningState ? hungryMaterial : rabbitMaterial;
        }

        private void SmoothMovement()
        {
            transform.position = Vector3.Lerp(transform.position, agent.nextPosition, Time.deltaTime * SMOOTHING_FACTOR);
        }

        private void AlignRotation()
        {
            if (agent.velocity != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, ROTATION_SPEED * Time.deltaTime);
            }
        }

        private IEnumerator Die()
        {
            agent.speed = 0;
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead_0", true);
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        public void JumpLandEvent()
        {
            landingParticleSystem.Play();
        }
    }
}


