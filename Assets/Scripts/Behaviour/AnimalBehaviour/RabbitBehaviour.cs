using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviour {
    public class RabbitBehaviour : MonoBehaviour
    {
        [SerializeField] private GrowthManager growthManager;
        [SerializeField] private float wanderRadius;
        [SerializeField] private Vector2 wanderTimerRange = new Vector2(3f, 7f);
        [SerializeField] private Vector2 idleDurationRange = new Vector2(2f, 5f);
        [SerializeField] private Vector2 speedRange = new Vector2(0.5f, 1.0f);
        [SerializeField] private float hungerThreshold;
        [SerializeField] private float warningThreshold;
        [SerializeField] private float deathThreshold;
        [SerializeField] private Material hungryMaterial;
        [SerializeField] private ParticleSystem landingParticleSystem;
        [SerializeField] private ParticleSystem rabbitGhostParticleSystem;

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
        private bool death = false;

        private const float FOOD_REACHED_THRESHOLD = 0.3f;
        private const float SMOOTHING_FACTOR = 5f;
        private const float ROTATION_SPEED = 360f;

        private void Start() {
            InitializeComponents();
            SwitchToIdle();
            EntityManager.Get().AddRabbit(gameObject);
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        private void OnDestroy() {
            EntityManager.Get().RemoveRabbit(gameObject);
        }

        private void FixedUpdate() {
            if (death) {
                agent.speed = 0;
            }

            MaterialChanger();
            HandleHunger();

            if (isHungry)
                FindClosestfFood();
            else
                HandleWanderAndIdle();

            SmoothMovement();
            AlignRotation();
        }

        private void InitializeComponents() {
            agent = GetComponent<NavMeshAgent>();
            InitializeAgentParameters();

            animator = GetComponent<Animator>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            rabbitMaterial = skinnedMeshRenderer.material;
        }

        private void InitializeAgentParameters() {
            agent.angularSpeed = 120f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 0.2f;
        }

        private void HandleHunger() {
            currentHunger += 5f * Time.fixedDeltaTime;

            if (currentHunger >= deathThreshold)
                Die();

            if (currentHunger >= hungerThreshold) {
                isHungry = true;

                if (currentHunger >= warningThreshold) {
                    LeafPointsSpawner.spawnLeafPoints = false;
                    inWarningState = true;
                }
            }
        }

        private void HandleWanderAndIdle() {
            stateTimer -= Time.fixedDeltaTime;

            if (stateTimer <= 0f) {
                if (isIdling)
                    SwitchToWander();
                else
                    SwitchToIdle();
            }
        }

        private void SwitchToWander() {
            isIdling = false;
            animator.SetBool("isJumping", true);
            Wander();
        }

        private void SwitchToIdle() {
            isIdling = true;
            animator.SetBool("isJumping", false);
            Idle();
        }

        private void Wander() {
            SetWanderParameters();

            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
            agent.SetDestination(navHit.position);
        }

        private void SetWanderParameters() {
            wanderTimer = Random.Range(wanderTimerRange.x, wanderTimerRange.y);
            idleDuration = 0f;
            stateTimer = wanderTimer;

            SetRandomSpeed();
        }

        private void SetRandomSpeed() {
            speed = Random.Range(speedRange.x, speedRange.y);
            agent.speed = speed;
        }

        public void Idle() {
            agent.speed = 0f;
            idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
            stateTimer = idleDuration;
        }

        private void FindClosestfFood() {
            List<GameObject> foods = EntityManager.Get().GetFoods();

            if (foods.Count == 0) {
                HandleNoFoodsFound();
                return;
            }

            Transform closestFood = FindClosestFoodTransform(foods);

            if (closestFood != null)
                HandleClosestFood(closestFood);
        }

        private void HandleNoFoodsFound() {
            animator.SetBool("isRunning", false);
            HandleWanderAndIdle();
        }

        private void HandleClosestFood(Transform closestFood) {
            if (Vector3.Distance(transform.position, closestFood.position) >= FOOD_REACHED_THRESHOLD) {
                MoveTowardsFood(closestFood);
            } else {
                EatFood(closestFood);
            }
        }

        private void MoveTowardsFood(Transform closestFood) {
            agent.speed = 1.3f;
            agent.SetDestination(closestFood.position);
            animator.SetBool("isRunning", true);
        }

        private void EatFood(Transform closestFood) {
            Food food = closestFood.GetComponent<Food>(); 
            food.Consume(); 

            animator.SetBool("isRunning", false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitEat");
            currentHunger -= 100f;
            growthManager.ProgressGrowth(food.foodGrowthValue);
            LeafPointsSpawner.spawnLeafPoints = true;
            isHungry = false;
            inWarningState = false;
            SetIdleAnimation();
        }

        private void SetIdleAnimation() {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            isIdling = true;
            HandleWanderAndIdle();
        }

        private Transform FindClosestFoodTransform(List<GameObject> foods) {
            Transform closestFood = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject food in foods) {
                Food foodObj = food.GetComponent<Food>(); //
                float distance = Vector3.Distance(transform.position, food.transform.position); //

                if (foodObj.CanBeConsumed() && distance < closestDistance) {
                    closestDistance = distance;
                    closestFood = food.transform;
                }
            }

            return closestFood;
        }

        private void MaterialChanger() {
            skinnedMeshRenderer.material = inWarningState ? hungryMaterial : rabbitMaterial;
        }

        private void SmoothMovement() {
            transform.position = Vector3.Lerp(transform.position, agent.nextPosition, Time.deltaTime * SMOOTHING_FACTOR);
        }

        private void AlignRotation() {
            if (agent.velocity != Vector3.zero) {
                Quaternion toRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, ROTATION_SPEED * Time.deltaTime);
            }
        }

        public void Die() {
            StartCoroutine(DeathSequence());
        }

        public void InstantDeath()
        {
            death = true;
            Destroy(gameObject, 2f);
            if (!rabbitGhostParticleSystem.isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
                rabbitGhostParticleSystem.Play();
                transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
            }
        }

        private IEnumerator DeathSequence() {
            death = true;
            Destroy(gameObject, 5f);
            animator.SetBool("isDead_0", true);
            yield return new WaitForSeconds(2f);
            if (!rabbitGhostParticleSystem.isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
                rabbitGhostParticleSystem.Play();
            }
            transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }

        public void JumpLandEvent() {
            landingParticleSystem.Play();
        }
    }
}


