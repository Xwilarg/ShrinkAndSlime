using LudumDare56.Player;
using UnityEngine;

namespace LudumDare56.Enemy
{
    public class Missile : MonoBehaviour
    {
        private bool _isAiming = true;

        private Rigidbody _rb;

        private void Awake()
        {
            Destroy(gameObject, 10f);
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isAiming)
            {
                transform.LookAt(transform.position + _rb.linearVelocity);
                transform.Rotate(-90f, 0f, 0f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<PlayerController>(out var pc))
            {
                pc.TakeDamage();
                Destroy(gameObject);
            }
            _isAiming = false;
        }
    }
}
