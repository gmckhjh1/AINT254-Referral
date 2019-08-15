using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace EnemyFactory
{
    //
    //Factory to create enemy objects
    //

    public static class EnemyFactory
    {
        
        private static IEnumerable<Type> m_implementedEnemies;//Implemented types of enemies
        private static bool isInitialised => m_implementedEnemies != null;//Validation variable
        private static List<string> m_enemiesByName = new List<string>();//Dictionary for enemy names
        private static List<GameObject> m_allEnemies = new List<GameObject>(); //Store list of gameobjects created by factory

        /// <summary>
        /// Automactically get all types of enemy.
        /// Using linq and reflection.
        /// This allows for expansion of enemy types without further
        /// changes to code bar creating the new class
        /// </summary>
        public static void InitialiseFactory()
        {
            //Check if factory is initilaised
            if (isInitialised)
            {
                return;
            }

            //Get IEnemy parent interface
            //Select the children of this assembly 
            Type parentType = typeof(IEnemy);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] enemyTypes = assembly.GetTypes();

            //determine which types implement the parent Interface and store them
            m_implementedEnemies = enemyTypes.Where(t => t.GetInterfaces().Contains(parentType));
                        
            //Select each enemytype and store name 
            foreach(var type in m_implementedEnemies)
            {                
                m_enemiesByName.Add(type.Name);
            }
        }
                
        /// <summary>
        /// Provide a collection of prefabs created with 
        /// classes from the Enemy Factory.
        /// ONLY TO BE CALLED ON START TO PREVENT PERFORMANCE ISSUES. 
        /// </summary>
        /// <param name="EnemyType"></param>
        /// <returns></returns>
        public static List<GameObject> GetEnemyTypes()
        {
            Debug.Log("Got to the enemyfactorytypes method");
            //Ensure factory is initialised
            InitialiseFactory();

            //If there are enemy types loop through and find 
            //Prefabs with the script in the resources folder
            //Ensures any new  tyes added will be automatically added.
            if (m_enemiesByName != null)
            {

                for(int i = 0; i < m_enemiesByName.Count(); i++)
                {
                    GameObject temp = (Resources.Load(m_enemiesByName[i]) as GameObject);                   
                    m_allEnemies.Add(temp);                    
                }
                
                return m_allEnemies;

            } else return null;                       
        }                                         
    }
}
