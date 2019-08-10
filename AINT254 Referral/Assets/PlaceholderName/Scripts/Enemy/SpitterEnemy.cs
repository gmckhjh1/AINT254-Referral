using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyFactory
{
    //
    //Spitter_Enemy inherits from abstract super class
    //Implements distinct methods relevant to this enemy
    //
    public class SpitterEnemy : MonoBehaviour, IEnemy, IDamageHandler
    {
        [SerializeField] private int health = 10;
        private float rangeAttackDistance = 5f;
        private bool isAttacking = false;
        public string Name => "SpitterEnemy";
        public int Health
        {
            get { return health; }
            private set { Health = value; }
        }

        [SerializeField] private GameObject playerRef;
        [SerializeField] private Animation attackAnim;
        [SerializeField] private SpitAttack weapon;

        private NavMeshAgent m_navAgent;

        void Start()
        {
            //Get navmeshagent component
            try { m_navAgent = gameObject.GetComponent<NavMeshAgent>(); }
            catch { Debug.Log("No navmeshagent component"); }
        }
        void Update()
        {
            CurrState();
        }
        
        /// <summary>
        /// Set current State
        /// </summary>
        public void CurrState()
        {
            BasicEnemyBehaviour();//Call basic enemy behaviour for prototyping                        
        }    

        /// <summary>
        /// Chase the player until attack distace is reached.
        /// Then call Attack method.
        /// </summary>
        private void BasicEnemyBehaviour()
        {           
            //Look at player if not null
            if (playerRef != null)
            {
                transform.LookAt(playerRef.transform);
            }
            else return;

            //Chase player until attack ditance is reached
            if(Vector3.Distance(transform.position, playerRef.transform.position) > rangeAttackDistance)
            {
                isAttacking = false;
                Attack();
                m_navAgent.enabled = true;                
                m_navAgent.destination = playerRef.transform.position;                
            }
            else
            {
                //Attack player if not already doing so
                if (!isAttacking)
                {
                    m_navAgent.enabled = false;
                    isAttacking = true;
                    Attack();                    
                }
                else return;
            }
        }

        /// <summary>
        /// Call to start and Stop attacking player
        /// </summary>
        public void Attack()
        {
            if (isAttacking)
            {
                weapon.StartAttack();
            }
            else
            {
                weapon.StopAttack();
            }
        }

        /// <summary>
        /// Add the enemy back to the enemyPool if it dies
        /// </summary>
        public void ReturnToPool()
        {
            //gameObject.SetActive(false);
           // EnemyPool.EnemyObjectPool.Instance.ReturnToPool(this.gameObject);
        }

        /// <summary>
        /// Implement the interface methd to take damage.
        /// Set gameobject to inactive when dead call ReturnToPool()
        /// </summary>
        /// <param name="Damage"></param>
        public void TakeDamage(int Damage)
        {
            Health -= Damage;

            if(Health <= 0)
            {
                //gameObject.SetActive(false);
                //ReturnToPool();
            }
        }
    }
}
