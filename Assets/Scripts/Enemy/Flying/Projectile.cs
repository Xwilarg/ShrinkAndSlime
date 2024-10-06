using LudumDare56.Player;
using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<PlayerController>(out var pc))
            {
                pc.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}
