using System;
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
        //Basic variables for this enemy type implemented from IEnemy
        [SerializeField] private int health = 10;             
        public string Name => "SpitterEnemy";
        public int Health
        {
            get { return health; }
            private set { Health = value; }
        }
        
        public float AttackDistance
        {
            get { return AttackDistance; }
        }
        public Player Player => Player.Instance;

        //Serialized references to attack features
        [SerializeField] private Animation attackAnim;
        [SerializeField] private SpitAttack weapon;
        [SerializeField] private Transform lineOfSight;
        [SerializeField] private ParticleSystem death;

        //Variables for state machine
        private EnemyState currentState;

        //Private variables for state selection
        private NavMeshAgent m_navAgent;
        private float rangeAttackDistance = 5f;
        private bool hasLineOfSight = false;

        void Start()
        {
            //Get navmeshagent component
            try { m_navAgent = gameObject.GetComponent<NavMeshAgent>(); }
            catch { Debug.Log("No navmeshagent component"); }

            StartState();
        }

        void Update()
        {
            //Look at player if not null
            if (Player != null)
            {
                transform.LookAt(Player.transform);
            }
            else return;

            GetStateAction();//Call to determine state machine action
        }

        //States for this enemy state machine
        public enum EnemyState
        {
            Chase,
            Attack
        }
        
        /// <summary>
        /// Set start state to chase()
        /// </summary>
        public void StartState()
        {
            currentState = EnemyState.Chase;
        }
        
        /// <summary>
        /// Set current State
        /// </summary>
        public void GetStateAction()
        {
            switch (currentState)
            {
                case EnemyState.Chase:
                    ChasePlayer();
                    break;

                case EnemyState.Attack:
                    Attack();
                    break;

                default:
                    Debug.Log("Can't get state");
                    break;
            }
        }    

        private void ChasePlayer()
        {
            hasLineOfSight = CheckLineOfSight();

            if (Vector3.Distance(transform.position, Player.transform.position) > rangeAttackDistance || !hasLineOfSight)
            {                
                m_navAgent.enabled = true;
                m_navAgent.destination = Player.transform.position;
            }
            else
            {
                m_navAgent.enabled = false;
                currentState = EnemyState.Attack;
            }
        }
        

        /// <summary>
        /// Call to start and Stop attacking player
        /// </summary>
        public void Attack()
        {
            hasLineOfSight = CheckLineOfSight();

            if (Vector3.Distance(transform.position, Player.transform.position) < rangeAttackDistance && hasLineOfSight)
            {                
                weapon.StartAttack();                
            }
            else
            {                
                weapon.StopAttack();
                currentState = EnemyState.Chase;
            }
        }

        private bool CheckLineOfSight()
        {
            RaycastHit checkSight;

            if (Physics.Raycast(lineOfSight.transform.position, lineOfSight.transform.forward,
                out checkSight, rangeAttackDistance))
            {
                if (checkSight.transform.tag == "Player")
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        /// <summary>
        /// Add the enemy back to the enemyPool if it dies
        /// </summary>
        public void ReturnToPool()
        {
            death.transform.position = transform.position;
            death.Play();//Play death particle effect

            gameObject.SetActive(false);
            EnemyPool.EnemyObjectPool.Instance.ReturnToPool(this.gameObject);
        }

        /// <summary>
        /// Implement the interface methd to take damage.
        /// Set gameobject to inactive when dead call ReturnToPool()
        /// </summary>
        /// <param name="Damage"></param>
        public void TakeDamage(int Damage, GameObject attackingEnemy)
        {
            health -= Damage;

            if(health <= 0)
            {
                gameObject.SetActive(false);
                death.transform.position = gameObject.transform.position;
                death.Play();                
                ReturnToPool();
            }
        }
    }
}
