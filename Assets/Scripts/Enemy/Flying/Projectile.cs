using LudumDare56.Player;
using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<PlayerController>(out var pc))
            {
                pc.TakeDamage();
            }
            Destroy(gameObject);
        }

        private void Update()
        {
            transform.LookAt(transform.position + _rb.linearVelocity);
            //transform.Rotate(-90f, 0f, 0f);
        }
    }
}
