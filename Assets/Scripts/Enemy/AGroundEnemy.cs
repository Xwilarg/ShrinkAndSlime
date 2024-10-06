using UnityEngine.AI;

namespace LudumDare56.Enemy
{
    public abstract class AGroundEnemy : AEnemyController
    {
        protected NavMeshAgent navMeshAgent;

        protected override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }
    }
}