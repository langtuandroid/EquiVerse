using System.Collections;
using System.Collections.Generic;
using Behaviour;
using DG.Tweening;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

public class FoxBehaviour : MonoBehaviour
{
        [SerializeField] private float wanderRadius;
        [SerializeField] private Vector2 wanderTimerRange = new Vector2(3f, 7f);
        [SerializeField] private Vector2 idleDurationRange = new Vector2(2f, 5f);
        [SerializeField] private Vector2 speedRange = new Vector2(0.5f, 1.0f);
        [SerializeField] private float hungerThreshold;
        [SerializeField] private float warningThreshold;
        [SerializeField] private float deathThreshold;
        [SerializeField] private Material hungryMaterial;
        [SerializeField] private ParticleSystem foxGhostParticleSystem;

        private SkinnedMeshRenderer skinnedMeshRenderer;
        private NavMeshAgent agent;
        private Animator animator;
        private Material foxMaterial;

        private float wanderTimer;
        private float idleDuration;
        private float speed;
        private float stateTimer;
        private bool isIdling = false;
        private bool isHungry = false;
        private bool inWarningState = false;
        private float currentHunger = 0f;
        private bool death = false;

        private const float FOOD_REACHED_THRESHOLD = 0.5f;
        private const float SMOOTHING_FACTOR = 5f;
        private const float ROTATION_SPEED = 360f;

        private void Start() {
            InitializeComponents();
            SwitchToIdle();
            EntityManager.Get().AddFox(gameObject);
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        private void OnDestroy() {
            EntityManager.Get().RemoveFox(gameObject);
        }

        private void FixedUpdate() {
            if (death)
            {
                agent.speed = 0;
            }

            MaterialChanger();
            HandleHunger();

            if (isHungry)
                FindClosestRabbit();
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
            foxMaterial = skinnedMeshRenderer.material;
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
                    //LeafPointsSpawner.spawnLeafPoints = false;
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
            animator.SetBool("isWalking", true);
            Wander();
        }

        private void SwitchToIdle() {
            isIdling = true;
            animator.SetBool("isWalking", false);
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

        private void FindClosestRabbit() {
            List<GameObject> rabbits = EntityManager.Get().GetRabbits();

            if (rabbits.Count == 0) {
                HandleNoRabbitsFound();
                return;
            }

            Transform closestRabbit = FindClosestRabbitTransform(rabbits);

            if (closestRabbit != null)
                HandleClosestRabbit(closestRabbit);
        }

        private void HandleNoRabbitsFound() {
            animator.SetBool("isRunning", false);
            HandleWanderAndIdle();
        }

        private void HandleClosestRabbit(Transform closestRabbit) {
            if (Vector3.Distance(transform.position, closestRabbit.position) >= FOOD_REACHED_THRESHOLD) {
                MoveTowardsRabbit(closestRabbit);
            } else {
                EatRabbit(closestRabbit);
            }
        }

        private void MoveTowardsRabbit(Transform closestFood) {
            agent.speed = 2.3f;
            agent.SetDestination(closestFood.position);
            animator.SetBool("isRunning", true);
        }

        private void EatRabbit(Transform closestRabbit) {
            RabbitBehaviour rabbitObj = closestRabbit.GetComponent<RabbitBehaviour>(); 
            rabbitObj.InstantDeath(); 
            animator.SetBool("isRunning", false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/FoxEat");
            currentHunger -= 100f;
            //LeafPointsSpawner.spawnLeafPoints = true;
            isHungry = false;
            inWarningState = false;
            SetIdleAnimation();
        }

        private void SetIdleAnimation() {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            isIdling = true;
            HandleWanderAndIdle();
        }

        private Transform FindClosestRabbitTransform(List<GameObject> rabbits) {
            Transform closestRabbit = null;
            float closestDistance = Mathf.Infinity;
        
            foreach (GameObject rabbit in rabbits) {
                float distance = Vector3.Distance(transform.position, rabbit.transform.position); //
                if ( distance < closestDistance) {
                    closestDistance = distance;
                    closestRabbit = rabbit.transform;
                }
            }
            return closestRabbit;
        }

        private void MaterialChanger() {
            skinnedMeshRenderer.material = inWarningState ? hungryMaterial : foxMaterial;
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

        private IEnumerator DeathSequence() {
            death = true;
            Destroy(gameObject, 5f);
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(2f);
            if (!foxGhostParticleSystem.isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
                foxGhostParticleSystem.Play();
            }
            transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }
}

