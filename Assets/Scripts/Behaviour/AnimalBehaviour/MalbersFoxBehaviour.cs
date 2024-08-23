using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MalbersAnimations;
using MalbersAnimations.Controller.AI;
using Managers;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

public class MalbersFoxBehaviour : MonoBehaviour
{
        [SerializeField] private CrystalShardSpawner crystalShardSpawner;
        [SerializeField] private float hungerThreshold;
        [SerializeField] private float warningThreshold;
        [SerializeField] private float deathThreshold;
        [SerializeField] private ParticleSystem foxGhostParticleSystem;
        [SerializeField] private Material hungryMaterial;
        public BoolVarListener isHungry;
        public MAnimalBrain animal;
        public MAIState deathState;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Material foxMaterial;
        private bool localIsHungry;
        private bool inWarningState = false;
        private float currentHunger = 0f;
    
        private void Start()
        {
            isHungry.Value = false;
            localIsHungry = false;
            EntityManager.Get().AddFox(gameObject);
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            foxMaterial = skinnedMeshRenderer.material;
        }
    
        private void FixedUpdate()
        {
            HandleHunger();
            MaterialChanger();
        }
    
        private void HandleHunger()
        {
            if (!EnemySpawner.enemyDanger)
            {
                currentHunger += (5f - UpgradeVariableController.foxHungerDecrementValue)* Time.fixedDeltaTime;
            }

            if (currentHunger >= deathThreshold)
                StartCoroutine(Die());
    
            if (currentHunger >= hungerThreshold  && EntityManager.Get().GetBabyRabbits().Count >= 1)
            {
                isHungry.Value = true;
                localIsHungry = true;
                CheckFoodDistance();
            }
            if (currentHunger >= warningThreshold && !inWarningState)
            {
                crystalShardSpawner.spawnCrystalShardPoints = false;
                inWarningState = true;
            }
        }
    
        public void EatFood(GameObject closestRabbit)
        {
            MalbersRabbitBehaviour rabbit = closestRabbit.GetComponent<MalbersRabbitBehaviour>();
            if (rabbit != null)
            {
                rabbit.InstantDeath(true);
                GameManager.animalDeaths--;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/FoxEat");
                currentHunger -= 100f;
                crystalShardSpawner.spawnCrystalShardPoints = true;
                inWarningState = false;
                isHungry.Value = false;
                localIsHungry = false;
            }
        }

        private void CheckFoodDistance()
        {
            if (animal.Target != null && localIsHungry)
            {
                float distance = Vector3.Distance(transform.position, animal.Target.position);
                if (distance < 1f)
                {
                    EatFood(animal.Target.gameObject);
                }
            }
        }
    
        public void InstantDeath()
        {
            GameManager.animalDeaths++;
            Destroy(gameObject, 3f);
            EntityManager.Get().RemoveFox(gameObject);
            if (!foxGhostParticleSystem.isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
                foxGhostParticleSystem.Play();
                transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
            }
        }
    
        private IEnumerator Die()
        {
            Destroy(gameObject, 3f);
            EntityManager.Get().RemoveFox(gameObject);
            animal.StartNewState(deathState);
            yield return new WaitForSeconds(2f);
            if (!foxGhostParticleSystem.isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/RabbitDeath");
                foxGhostParticleSystem.Play();
                GameManager.animalDeaths++;
            }
            transform.DOScale(0, 0.5f).SetEase(Ease.OutBack);
        }
    
        private void MaterialChanger()
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = inWarningState ? hungryMaterial : foxMaterial;
            }
        }
}
