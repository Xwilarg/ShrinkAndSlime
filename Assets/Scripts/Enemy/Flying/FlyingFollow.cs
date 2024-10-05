using LudumDare56.Player;
using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class FlyingFollow : AFlyingEnemy
    {
        private void FixedUpdate()
        {
            var revDir = PlayerController.Instance.transform.position + ((transform.position - PlayerController.Instance.transform.position).normalized * 5f) - (Vector3.one * 5f);
            revDir.y = 5f;

            var prev = transform.eulerAngles;
            transform.LookAt(PlayerController.Instance.transform);
            transform.eulerAngles = new(prev.x, transform.eulerAngles.y, prev.z);

            MoveTowards(revDir, 5f);
        }
    }
}
