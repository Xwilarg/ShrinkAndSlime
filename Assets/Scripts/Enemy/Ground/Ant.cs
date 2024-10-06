using UnityEngine;

namespace LudumDare56.Enemy.Ground
{
    public class Ant : AGroundEnemy
    {
        protected override void Start()
        {
            base.Start();

            navMeshAgent.SetDestination(_targetNode.transform.position);
        }

        private void Update()
        {
            var didReachDest = !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
            if (_fightingTarget != null)
            {
                _animator.SetBool("IsMoving", !didReachDest);
                navMeshAgent.SetDestination(_fightingTarget.transform.position);
            }
            else
            {
                if (didReachDest)
                {
                    _targetNode = _targetNode.NextNode;
                    navMeshAgent.SetDestination(_targetNode.transform.position);
                }
            }
        }
    }
}