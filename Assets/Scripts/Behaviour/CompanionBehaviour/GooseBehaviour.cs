using UnityEngine;

namespace Behaviour
{
    public class GooseBehaviour : MonoBehaviour
    {
        private Animator animator;

        public GameObject eggPrefab;
        public Transform eggSpawnPosition;

        public float secondIdleTriggerMinWait = 5f;
        public float secondIdleTriggerMaxWait = 15f;

        void Start()
        {
            animator = GetComponent<Animator>();
            //Erwin: Leuk dat je Invoke gebruikt! Voor deze toepassing naar mijn idee zeer geschikt.
            //Over het algemeen worden in Unity hier vaker coroutines voor gebruikt voor makkelijkere debugging, readability en scalability
            Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
            Invoke("EggDropTrigger", Random.Range(15f, 45f));
        }

        void TriggerSecondIdle()
        {
            animator.SetTrigger("SecondIdleTrigger");
            Invoke("TriggerSecondIdle", Random.Range(secondIdleTriggerMinWait, secondIdleTriggerMaxWait));
        }

        void EggDropTrigger()
        {
            animator.SetTrigger("EggDropTrigger");
            Instantiate(eggPrefab, eggSpawnPosition);
            Invoke("EggDropTrigger", Random.Range(15f, 45f));
        }
    }
}
