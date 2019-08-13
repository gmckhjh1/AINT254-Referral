using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFactory
{
    //
    //Superclass interface implementation for all enemies
    //
    public interface IEnemy
    {
        /// <summary>
        /// Ensure that this name is the same as the class name!
        /// </summary>
        string Name { get; }
        Player Player { get; }
        StateMachine StateMachine { get; }
        int Health { get; }
        void CurrState();
        void Attack();
        void ReturnToPool();
        void InitialiseStateMachine();
    }
       
}
