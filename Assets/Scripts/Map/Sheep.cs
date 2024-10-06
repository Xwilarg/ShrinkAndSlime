using LudumDare56.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Map
{
    public class Sheep : MonoBehaviour, IScalable
    {
        public GameObject GameObject => gameObject;

        public float BaseScale { private set; get; }

        public float ScaleProgression { get; set; }

        public NavMeshAgent Agent { private set; get; }

        private void Awake()
        {
            BaseScale = transform.localScale.x;
            Agent = GetComponent<NavMeshAgent>();
        }
    }
}
