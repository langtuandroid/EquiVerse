using UnityEngine;

namespace Behaviour
{
    public class GooseBehaviour : MonoBehaviour
    {
        private Animator animator;

        public GameObject eggPrefab;
        public Transform eggSpawnPosition;
        public ParticleSystem featherParticles;

        public float secondIdleTriggerMinWait = 5f;
        public float secondIdleTriggerMaxWait = 15f;

        public float eggDropMinWait = 45f;
        public float eggDropMaxWait = 60f;
        
        

        void Start()
        {
            animator = GetComponent<Animator>();
            Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
            Invoke("EggDropTrigger", Random.Range(eggDropMinWait, eggDropMaxWait));
        }

        void TriggerSecondIdle()
        {
            animator.SetTrigger("SecondIdleTrigger");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/FerdinandGoose/FerdinandHonk");
            Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
        }

        void EggDropTrigger()
        {
            animator.SetTrigger("EggDropTrigger");
            featherParticles.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Animals/FerdinandGoose/FerdinandEggDrop");
            Instantiate(eggPrefab, eggSpawnPosition);
            Invoke("EggDropTrigger", Random.Range(eggDropMinWait, eggDropMaxWait));
        }
    }
}
