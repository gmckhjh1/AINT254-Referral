using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAttack : MonoBehaviour
{
    [SerializeField] private int attackPower = 5;//Set attack power
    [SerializeField] private float attackReload = 3f;//Set wait time between attacks
    [SerializeField] private float attackLength = 2f;//Set length of attack
    [SerializeField] private ParticleSystem particleSystem;//Particle system ref
    Coroutine lastCoroutine;

    public int AttackPower
    {
        get { return attackPower; }
        private set { attackPower = value; }
    }

    public float AttackReload
    {
        get { return attackReload; }
        private set { attackReload = value; }
    }

    private void Start()
    {
        particleSystem.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// public method to allow other classes to call the attack.
    /// </summary>
    public void StartAttack()
    {
        particleSystem.Play();
    }

    /// <summary>
    /// Public method to allow attack to be stopped.
    /// Also stop particle system as it will finish playing it's current cycle.
    /// </summary>
    public void StopAttack()
    {
        particleSystem.Stop();
    }
    
    /// <summary>
    /// If particle system collides with player then deal damage 
    /// using the IDamageHandler. Turn off particle system. 
    /// </summary>
    /// <param name="other"></param>

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("in particle collision method");
        
        if (other.tag == "Enemy")
        {
            Debug.Log(other);
            //Call IDamageHandler
            IDamageHandler canTakeDamage = other.GetComponent<IDamageHandler>();
            if (canTakeDamage != null)
            {
                canTakeDamage.TakeDamage(attackPower, transform.parent.gameObject);
            }

            particleSystem.Stop();
        }
        else return;
    }
}
