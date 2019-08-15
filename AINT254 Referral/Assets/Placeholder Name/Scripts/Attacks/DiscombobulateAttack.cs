﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//
//An attack that implements attacks interface
//
public class DiscombobulateAttack : MonoBehaviour
{
    [SerializeField] private int m_attackPower = 5;//Set attack power
    [SerializeField] private float m_attackReload = 3f;//Set wait time between attacks
    [SerializeField] private float m_attackLength = .5f;//Set length of attack
    [SerializeField] private ParticleSystem m_particleSystem;//Particle system ref
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
        m_particleSystem.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// public method to allow other classes to call the attack.
    /// </summary>
    public void StartAttack()
    {
        lastCoroutine = StartCoroutine(Attacking());
    }

    /// <summary>
    /// Public method to allow attack to be stopped.
    /// Also stop particle system as it will finish playing it's current cycle.
    /// </summary>
    public void StopAttack()
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
            m_particleSystem.Stop();
        }
        else return;
    }

    /// <summary>
    /// Attacking coroutine loop. 
    /// Plays continuously until interrupted by the calling class.
    /// </summary>
    /// <returns></returns>
    IEnumerator Attacking()
    {
        m_particleSystem.Play();
        yield return new WaitForSeconds(m_attackLength);

        m_particleSystem.Stop();
        yield return new WaitForSeconds(m_attackReload);

        StartAttack();
    }

    /// <summary>
    /// If particle system collides with player then deal damage 
    /// using the IDamageHandler. Turn off particle system. 
    /// </summary>
    /// <param name="m_other"></param>

    private void OnParticleCollision(GameObject m_other)
    {
        
        if (m_other.tag == "Player")
        {
            //Call IDamageHandler
            IDamageHandler canTakeDamage = m_other.GetComponent<IDamageHandler>();
            if (canTakeDamage != null)
            {
                canTakeDamage.TakeDamage(m_attackPower, transform.parent.gameObject);
            }

            //Add force to player collider to knock them backwardss
            m_other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-5f, 0f, 0f), ForceMode.Impulse);
            
            m_particleSystem.Stop();//Turn off particle system
        }
        else return;
    }
}
