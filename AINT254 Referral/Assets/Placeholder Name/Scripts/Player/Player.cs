using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, IDamageHandler
{
    //Basic player references
    [SerializeField] private int m_health = 50;//Player health before death
    [SerializeField] private FlamethrowerAttack m_weapon;//Weapon reference

    //Damage effects references and variables
    [SerializeField] private Camera m_mainCam;//Regular camera
    [SerializeField] private Camera m_effectsCam;//Camera reference for damage effects
    private CameraEffects m_shakeCam;//Reference to camEffects script
    [SerializeField] private float m_effectLength = 3f;
    [SerializeField] private float m_shakeMag = 0.02f;

    //Reference to the controller script
    private RigidbodyFirstPersonController m_controller;

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
        
        
        //Try to get cameraEffects script attached to effectCam
        try
        {
            m_shakeCam = m_effectsCam.GetComponent<CameraEffects>();
        }
        catch
        {
            Debug.Log("No reference to cameraEffects for player damage effects");
        }
        

        try
        {
            m_controller = GetComponent<RigidbodyFirstPersonController>();
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
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage, GameObject _attackingObject)
    {        
        switch (_attackingObject.name)
        {
            case "SpitterEnemy":
                DamagedEffectsSpitter();
                break;

            case "SpitterEnemy(Clone)":
                DamagedEffectsSpitter();
                break;

            case "DiscombobulateEnemy":
                DamagedEffectsDiscombob();
                break;

            case "DiscombobulateEnemy(Clone)":
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
        m_mainCam.enabled = false;
        m_effectsCam.enabled = true;
        m_shakeCam.InititiateEffects(m_effectLength, m_shakeMag);

        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                
    }

    /// <summary>
    /// Call damage effects method from controller when damaged
    /// </summary>
    private void DamagedEffectsDiscombob()
    {
        m_controller.InitiateDamageEffects();
    }

    /// <summary>
    /// Return cameras to undamaged state
    /// </summary>
    public void CamSwitchNormal()
    {
        m_mainCam.enabled = true;
        m_effectsCam.enabled = false;        
    }

   /// <summary>
   /// Start attack for attached weapon
   /// </summary>
    public void StartAttack()
    {
        m_weapon.StartAttack();
    }

    /// <summary>
    /// Stop attack fo attached weapon
    /// </summary>
    public void StopAttack()
    {
        m_weapon.StopAttack();
    }
}
