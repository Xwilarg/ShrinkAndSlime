using LudumDare56.Player;
using System.Collections;
using UnityEngine;

namespace LudumDare56.Enemy
{
    public class MissileRat : AEnemyController
    {
        [SerializeField]
        private GameObject _missile;

        [SerializeField]
        private Transform _missileOutPoint;

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(Shoot());
        }

        private void Update()
        {
            var prev = _modelContainer.eulerAngles;
            _modelContainer.LookAt(PlayerController.Instance.transform, Vector3.up);
            _modelContainer.eulerAngles = new(prev.x, _modelContainer.transform.eulerAngles.y, prev.z);
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);

                var missile = Instantiate(_missile, _missileOutPoint.position, Quaternion.identity);
                var dir = (PlayerController.Instance.transform.position - transform.position).normalized;
                dir.x += Random.Range(-.5f, .5f);
                dir.z += Random.Range(-.5f, .5f);

                dir.y += Random.Range(0f, 2f);

                missile.GetComponent<Rigidbody>().AddForce(dir * Random.Range(10f, 30f), ForceMode.Impulse);
            }
        }
    }
}
