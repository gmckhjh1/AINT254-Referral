using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//A flamethrower weapon that implements attacks interface
//
public class FlamethrowerAttack : MonoBehaviour
{
    [SerializeField] private int m_attackPower = 5;//Set attack power
    [SerializeField] private float m_attackReload = 3f;//Set wait time between attacks
    [SerializeField] private float m_attackLength = 2f;//Set length of attack
    [SerializeField] ParticleSystem m_particleSystem;//Particle system ref
    Coroutine lastCoroutine;

    public int AttackPower
    {
        get { return m_attackPower; }
        private set { m_attackPower = value; }
    }

    public float AttackReload
    {
        get { return m_attackReload; }
        private set { m_attackReload = value; }
    }

    private void Start()
    {
       // m_particleSystem.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// public method to allow other classes to call the attack.
    /// </summary>
    public void StartAttack()
    {
        m_particleSystem.Play();
    }

    /// <summary>
    /// Public method to allow attack to be stopped.
    /// Also stop particle system as it will finish playing it's current cycle.
    /// </summary>
    public void StopAttack()
    {
        m_particleSystem.Stop();
    }
    
    /// <summary>
    /// If particle system collides with player then deal damage 
    /// using the IDamageHandler. Turn off particle system. 
    /// </summary>
    /// <param name="m_other"></param>

    private void OnParticleCollision(GameObject m_other)
    {
        Debug.Log("in particle collision method");
        
        if (m_other.tag == "Enemy")
        {
            Debug.Log(m_other);
            //Call IDamageHandler
            IDamageHandler canTakeDamage = m_other.GetComponent<IDamageHandler>();
            if (canTakeDamage != null)
            {
                canTakeDamage.TakeDamage(m_attackPower, transform.parent.gameObject);
            }

            m_particleSystem.Stop();
        }
        else return;
    }
}
