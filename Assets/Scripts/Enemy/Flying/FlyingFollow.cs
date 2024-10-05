using LudumDare56.Player;
using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class FlyingFollow : AFlyingEnemy
    {
        private void FixedUpdate()
        {
            var revDir = PlayerController.Instance.transform.position + ((transform.position - PlayerController.Instance.transform.position).normalized * 5f) - (Vector3.one * 5f * (1f - ScaleProgression + .1f));
            revDir.y = 5f * (1f - ScaleProgression + .1f);

            var prev = _model.eulerAngles;
            _model.LookAt(PlayerController.Instance.transform, Vector3.up);
            _model.eulerAngles = new(prev.x, _model.transform.eulerAngles.y, prev.z);

            if (Vector3.Distance(transform.position, revDir) < .1f)
            {
                MoveTowards(transform.position, 0f);
            }
            else
            {
                MoveTowards(revDir, 5f);
            }
        }
    }
}
