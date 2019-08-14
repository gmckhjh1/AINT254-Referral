using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyPool
{
    //
    //Monobehaviour to periodically spawn enemies at a location
    //
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnTimer = 3.0f;//Set how oten enemy spawns
        EnemyObjectPool enemyPool;//Reference to the enemy pool on this object

        private void Start()
        {
            enemyPool = GetComponent<EnemyObjectPool>();//Initialise enemy pool
            StartSpawner();
        }
         
        /// <summary>
        /// Call spawner coroutine
        /// </summary>
        private void StartSpawner()
        {
            StartCoroutine(spawnCountdown());//Start periodic enemyspawning
        }
        /// <summary>
        /// Start coroutine to regularly call SpawnEnemy method
        /// </summary>
        /// <returns></returns>
        private IEnumerator spawnCountdown()
        {            
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnTimer);
            StartSpawner();
        }

        /// <summary>
        /// Call spawn random enemy method from EnemyObjectPool
        /// </summary>
        void SpawnRandomEnemy()
        {            
            if (enemyPool)
            {
                var Enemy = enemyPool.GetRandomEnemy();
                Enemy.transform.position = transform.position;
                Enemy.SetActive(true);
            }
        }
    }
}
