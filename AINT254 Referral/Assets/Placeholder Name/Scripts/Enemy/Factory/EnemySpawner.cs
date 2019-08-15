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
        [SerializeField] private float m_spawnTimer = 3.0f;//Set how oten enemy spawns
        //[SerializeField] EnemyObjectPool m_enemyPool;//Reference to the enemy pool on this object
        Coroutine m_lastRoutine;

        private List<GameObject> m_spawnedObjects = new List<GameObject>();

        private GameObject spawnEnemy;

        private void Start()
        {            
            StartSpawner();
        }
         
        /// <summary>
        /// Call spawner coroutine
        /// </summary>
        private void StartSpawner()
        {
            Debug.Log("Start spawner method");
            StartCoroutine(spawnCountdown());//Start periodic enemyspawning
        }
        /// <summary>
        /// Start coroutine to regularly call SpawnEnemy method
        /// </summary>
        /// <returns></returns>
        private IEnumerator spawnCountdown()
        {
            Debug.Log("In iENumerator");
            SpawnRandomEnemy();
            yield return new WaitForSeconds(m_spawnTimer);
            StartSpawner();
        }

        /// <summary>
        /// Call spawn random enemy method from EnemyObjectPool
        /// </summary>
        void SpawnRandomEnemy()
        {
            Debug.Log("In spawn random");
            //Debug.Log(m_enemyPool);

            //if (m_enemyPool)
            //{
                Debug.Log("Got enemy pool");
                spawnEnemy = EnemyObjectPool.Instance.GetRandomEnemy();
               // m_spawnedObjects.Add(EnemyObjectPool.Instance.GetRandomEnemy());
                Debug.Log(spawnEnemy);
                spawnEnemy.gameObject.SetActive(false);
                spawnEnemy.transform.position = transform.position;
                Debug.Log(spawnEnemy.transform.position);
            //}
        }
    }
}
