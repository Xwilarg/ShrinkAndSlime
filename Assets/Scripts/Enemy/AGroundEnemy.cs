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
    }
}