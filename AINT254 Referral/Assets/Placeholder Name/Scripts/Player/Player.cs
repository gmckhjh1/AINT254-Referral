﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, IDamageHandler
{
    //Basic player references
    [SerializeField] private int health = 50;//Player health before death
    [SerializeField] private FlamethrowerAttack weapon;//Weapon reference

    //Damage effects references and variables
    [SerializeField] private Camera mainCam;//Regular camera
    [SerializeField] private Camera effectsCam;//Camera reference for damage effects
    private CameraEffects shakeCam;//Reference to camEffects script
    [SerializeField] private float effectLength = 3f;
    [SerializeField] private float shakeMag = 0.02f;

    //Reference to the controller script
    private RigidbodyFirstPersonController controller;

    //Singleton player 
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
        
        /*
        //Try to get cameraEffects script attached to effectCam
        try
        {
            shakeCam = effectsCam.GetComponent<CameraEffects>();
        }
        catch
        {
            Debug.Log("No reference to cameraEffects for player damage effects");
        }
        */

        try
        {
            controller = GetComponent<RigidbodyFirstPersonController>();
        }
        catch
        {
            Debug.Log("No FPSController reference for dmage effects");
        }
    }
      

    /// <summary>
    /// Implement Interface TakeDamage method.
    /// Select how player is damaged based on type of enemy.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, GameObject attackingObject)
    {        
        switch (attackingObject.name)
        {
            case "SpitterEnemy":
                DamagedEffectsSpitter();
                break;

            case "DiscombobulateEnemy":
                DamagedEffectsDiscombob();
                break;

            default:
                Debug.Log("Can't find damage effects for player");
                break;
        }  
    }

    /// <summary>
    /// Effects on the player when attacked
    /// </summary>
    private void DamagedEffectsSpitter()
    {        
        mainCam.enabled = false;
        effectsCam.enabled = true;
        shakeCam.InititiateEffects(effectLength, shakeMag);

        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                
    }

    /// <summary>
    /// Call damage effects method from controller when damaged
    /// </summary>
    private void DamagedEffectsDiscombob()
    {
        controller.InitiateDamageEffects();
    }

    /// <summary>
    /// Return cameras to undamaged state
    /// </summary>
    public void CamSwitchNormal()
    {
        mainCam.enabled = true;
        effectsCam.enabled = false;        
    }

   /// <summary>
   /// Start attack for attached weapon
   /// </summary>
    public void StartAttack()
    {
        weapon.StartAttack();
    }

    /// <summary>
    /// Stop attack fo attached weapon
    /// </summary>
    public void StopAttack()
    {
        weapon.StopAttack();
    }
}
