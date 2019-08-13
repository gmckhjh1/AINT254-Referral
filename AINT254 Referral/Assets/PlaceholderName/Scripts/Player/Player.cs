using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.PostProcessing;

public class Player : MonoBehaviour, IDamageHandler
{
    [SerializeField] private int health = 50;//Player health before death
    [SerializeField] private GameObject weapon;//Weapon reference

    [SerializeField] private Camera mainCam;//Regular camera
    [SerializeField] private Camera effectsCam;//Camera reference for damage effects
    private CameraEffects shakeCam;//Reference to camEffects script
    [SerializeField] private float effectLength = 3f;
    [SerializeField] private float shakeMag = 0.02f;

    //Enum, get and set for playerstate
    public enum PlayerStates { Normal, Damaged }

    public PlayerStates currState;
    
    public PlayerStates state
    {
        get { return state; }
        set { currState = value; }
    }
    

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
            shakeCam = effectsCam.GetComponent<CameraEffects>();
        }
        catch
        {
            Debug.Log("No reference to cameraEffects for player damage effects");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set player state to normal
        currState = PlayerStates.Normal;
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
        currState = PlayerStates.Damaged;
        DamagedEffects();
        
    }

    /// <summary>
    /// Effects on the player when attacked
    /// </summary>
    private void DamagedEffects()
    {
        mainCam.enabled = false;
        effectsCam.enabled = true;
        shakeCam.InititiateEffects(effectLength, shakeMag);

        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                
    }

    public void CamSwitchNormal()
    {
        
        //if (currState == PlayerStates.Normal)
        //{
            mainCam.enabled = true;
            effectsCam.enabled = false;
        //}
        //else return;
        
    }
}
