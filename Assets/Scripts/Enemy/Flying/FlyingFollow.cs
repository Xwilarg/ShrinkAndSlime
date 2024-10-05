using LudumDare56.Player;
using System.Collections;
using UnityEngine;

namespace LudumDare56.Enemy.Flying
{
    public class FlyingFollow : AFlyingEnemy
    {
        [SerializeField]
        private GameObject _projectilePrefab;

        [SerializeField]
        private Transform _gunEnd;

        private bool _canShoot = true;

        private void FixedUpdate()
        {
            Transform lookTarget;
            if (_fightingTarget == null)
            {
                if (_targetNode == null)
                {
                    MoveTowards(transform.position, 0f);
                }
                else
                {
                    lookTarget = _targetNode.transform;

                    var a = new Vector2(_targetNode.transform.position.x, _targetNode.transform.position.z);
                    var b = new Vector2(transform.position.x, transform.position.z);
                    if (Vector2.Distance(a, b) < .1f)
                    {
                        _targetNode = _targetNode.NextNode;
                        if (_targetNode == null)
                        {
                            MoveTowards(transform.position, 0f);
                            return;
                        }
                    }
                    var t = _targetNode.transform.position;
                    t.y = 7.5f;
                    MoveTowards(t, 5f);
                }
            }
            else
            {
                lookTarget = PlayerController.Instance.transform;

                var revDir = PlayerController.Instance.transform.position + ((transform.position - PlayerController.Instance.transform.position).normalized * 5f) - (Vector3.one * 5f * (1f - ScaleProgression + .1f));
                revDir.y = 5f * (1f - ScaleProgression + .1f);

                if (Vector3.Distance(transform.position, revDir) < .1f)
                {
                    MoveTowards(transform.position, 0f);
                }
                else
                {
                    MoveTowards(revDir, 5f);
                }

                if (_canShoot)
                {
                    var go = Instantiate(_projectilePrefab, _gunEnd);
                    go.GetComponent<Rigidbody>().linearVelocity = (PlayerController.Instance.transform.position - transform.position).normalized * 20f;
                    StartCoroutine(Reload());
                }
            }

            var prev = _modelContainer.eulerAngles;
            _modelContainer.LookAt(lookTarget, Vector3.up);
            _modelContainer.eulerAngles = new(prev.x, _modelContainer.transform.eulerAngles.y, prev.z);
        }

        private IEnumerator Reload()
        {
            _canShoot = false;
            yield return new WaitForSeconds(3f);
            _canShoot = true;
        }
    }
}
