using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnemyPool
{
    //
    //Class to pool enemy objects of various types.
    //
    public class EnemyObjectPool : MonoBehaviour
    {
        private List<GameObject> enemies;//List of all enemy objects        
        private static System.Random rand = new System.Random();
        [SerializeField] private int initialPoolSize = 10;

        //Queue to store pooled enemyObjs
        //Random objects usually selected due to potential different enemy types
        //but Queue still in use in case of later change
        private Queue<GameObject> pooledEnemiesAvailable = new Queue<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            enemies = EnemyFactory.EnemyFactory.GetEnemyTypes();//Initlaise list of enemyobjects
            InitialisePool();
        }

        /// <summary>
        /// Initialise Object pool with at least one enemyObj
        /// </summary>
        private void InitialisePool()
        {
            if(initialPoolSize <= 0)
            {
                AddToPool();
            }
            else
            {
                for(int i = 0; i < initialPoolSize; i++)
                {
                    AddToPool();
                }
            }
        }

        /// <summary>
        /// Return Random enemy gameObj.
        /// if one doesnt exist then add to the pool and then return. 
        /// </summary>
        /// <returns></returns>
        public GameObject GetRandomEnemy()
        {
            GameObject spawnEnemy;
            if(pooledEnemiesAvailable != null)
            {
                spawnEnemy = RandEnemyHelper();
                return spawnEnemy;
            }
            else
            {
                AddToPool();
                spawnEnemy = RandEnemyHelper();
                return spawnEnemy;
            }
        }       

        /// <summary>
        /// Add a random prefab into object pool
        /// </summary>
        private void AddToPool()
        {
            //Get random prefab
            GameObject enemy = RandEnemyHelper();

            //Instantiate and disable
            Instantiate(enemy, transform.position, transform.rotation);
            enemy.SetActive(false);

            //Add to pool and return
            pooledEnemiesAvailable.Enqueue(enemy);
        }

        /// <summary>
        /// Return Gameobject to pool
        /// </summary>
        /// <param name="enemy"></param>
        public void ReturnToPool(GameObject enemy)
        {
            pooledEnemiesAvailable.Enqueue(enemy);
        }
                
        /// <summary>
        /// Private helper to get next random enemy available in pool
        /// </summary>
        /// <returns></returns>
        private GameObject RandEnemyHelper()
        {
            return enemies[rand.Next(0, enemies.Count)];
        }
    }
}
