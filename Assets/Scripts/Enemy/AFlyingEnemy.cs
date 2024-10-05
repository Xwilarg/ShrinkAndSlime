using UnityEngine;

namespace LudumDare56.Enemy
{
    public abstract class AFlyingEnemy : AEnemyController
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected void MoveTowards(Vector3 target, float speed)
        {
            _rb.linearVelocity = (target - transform.position).normalized * speed;
        }
    }
}
