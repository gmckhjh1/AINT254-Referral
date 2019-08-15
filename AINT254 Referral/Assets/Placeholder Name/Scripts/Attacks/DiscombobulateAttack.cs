using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiscombobulateAttack : MonoBehaviour
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
            particleSystem.Stop();
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
        Debug.Log("Is attacking");
        particleSystem.Play();
        yield return new WaitForSeconds(attackLength);

        particleSystem.Stop();
        yield return new WaitForSeconds(attackReload);

        StartAttack();
    }

    /// <summary>
    /// If particle system collides with player then deal damage 
    /// using the IDamageHandler. Turn off particle system. 
    /// </summary>
    /// <param name="other"></param>

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("In collision ");

        Debug.Log(other);
        if (other.tag == "Player")
        {
            //Call IDamageHandler
            IDamageHandler canTakeDamage = other.GetComponent<IDamageHandler>();
            if (canTakeDamage != null)
            {
                canTakeDamage.TakeDamage(attackPower, transform.parent.gameObject);
            }

            //Add force to player collider to knock them backwardss
            other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-5f, 0f, 0f), ForceMode.Impulse);
            //Turn on damage effect panel

            particleSystem.Stop();//Turn off particle system
        }
        else return;
    }
}
