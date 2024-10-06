using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public abstract class AGroundEnemy : AEnemyController
    {
        NavMeshAgent navMeshAgent;

        protected override void Awake()

        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_fightingTarget != null)
            {
                navMeshAgent.SetDestination(_fightingTarget.transform.position);
                _animator.SetBool("IsMoving", true);
            }
                
        }
    }
}