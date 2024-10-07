using UnityEngine;
namespace LudumDare56.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private float _minSpawnTimer = 2f;
        [SerializeField] private float _maxSpawnTimer = 5f;
        [SerializeField] private int _maxEnemyCount = 4; //max amount of enemies out and about at once
        private float spawnTimer;
        private float _timeTilSpawn;

        // Update is called once per frame
        void Update()
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= _timeTilSpawn)
            {
                if(GetEnemyCount() < _maxEnemyCount)
                {
                    SpawnRandomEnemy();
                }
                spawnTimer = 0;
                GetNewSpawnTime(); // new timer for when the next enemy will spawn ( it is random )
            }
        }

        private void SpawnEnemy(GameObject enemy)
        {
            Instantiate(enemy,transform.position,transform.rotation,transform);
        }

        private void SpawnRandomEnemy()
        {
            var randNum = Random.Range(0, _enemyPrefabs.Length);

            SpawnEnemy(_enemyPrefabs[randNum]);
        }

        private int GetEnemyCount()
        {
            var enemies = FindObjectsOfType<AEnemyController>();
            return FindObjectsOfType<AEnemyController>().Length;
        }

        private void GetNewSpawnTime()
        {
            _timeTilSpawn = Random.Range(_minSpawnTimer, _maxSpawnTimer);
        }

    }
}