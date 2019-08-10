using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.PostProcessing;

public class Player : MonoBehaviour, IDamageHandler
{
    [SerializeField] private int health = 50;
    [SerializeField] private GameObject weapon;

    private static Player sInstance = null;
    public static Player Instance
    {
        get
        {
            if(sInstance == null)
            {
                GameObject singleton = new GameObject();
                sInstance = singleton.AddComponent<Player>();                
            }
            return sInstance;
        }

    }

    private void Awake()
    {
        //Set this as singleton Player if there isn't already one
        if (sInstance == null)
        {
            sInstance = this;
            name = "Player";
        }
        else
        {
            enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Rendering.PostProcessing.MotionBlur motion = new UnityEngine.Rendering.PostProcessing.MotionBlur();

        motion.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Implement Interface TakeDamage method
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        /*
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        */
        DamagedEffects();
    }

    private void DamagedEffects()
    {
        
    }
}
