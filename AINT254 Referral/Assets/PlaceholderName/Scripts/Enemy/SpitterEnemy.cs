using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFactory
{
    //
    //Spitter_Enemy inherits from abstract super class
    //Implements distinct methods relevant to this enemy
    //
    public class SpitterEnemy : MonoBehaviour, IEnemy
    {        
        public string Name => "Spitter";
        public int Health => 10;

        
        public void Attack()
        {
            Debug.Log("This is my attack");
        }
        public void CurrState()
        {
            Debug.Log("Implement the CurrState method");
        }    
    }
}
