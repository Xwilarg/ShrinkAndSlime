using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}
