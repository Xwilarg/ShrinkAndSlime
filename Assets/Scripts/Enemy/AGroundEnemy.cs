using LudumDare56.Player;
using UnityEngine;
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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<PlayerController>(out var pc))
            {
                pc.TakeDamage();
            }
        }
    }
}