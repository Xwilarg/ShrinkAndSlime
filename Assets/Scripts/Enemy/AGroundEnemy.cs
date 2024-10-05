using UnityEngine;
using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public abstract class AGroundEnemy : AEnemyController
    {
        NavMeshAgent navMeshAgent;

        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}