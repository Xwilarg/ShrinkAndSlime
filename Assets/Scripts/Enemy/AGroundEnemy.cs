using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public abstract class AGroundEnemy : AEnemyController
    {
        NavMeshAgent navMeshAgent;

        public GameObject target;

        protected override void Awake()

        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (target != null)
                navMeshAgent.SetDestination(target.transform.position);
        }
    }
}