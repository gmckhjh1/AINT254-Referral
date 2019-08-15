using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//Class to give functionality to end level markers
//

public class EndLevelMarker : MonoBehaviour
{  
    private Animator m_anim;//Reference to animator

    private void Start()
    {
        m_anim = GetComponent<Animator>();//Get Reference to animator
        
        //Check reference isn't null
        if (m_anim != null)
        {            
            StartAnim();//Call flash coroutine
        }    
    }

    /// <summary>
    /// Method to call coroutine.
    /// Used to keep code clean so coroutine isn't calling itself.
    /// </summary>
    void StartAnim()
    {
        StartCoroutine(Flash());
    }

    /// <summary>
    /// Make game object flash for a breif period.
    /// Wait for 5 seconds and repeat
    /// </summary>
    /// <returns></returns>
    IEnumerator Flash()
    {
        m_anim.enabled = true;
        yield return new WaitForSeconds(5f); //Play animation for 5 seconds

        m_anim.enabled = false;
        yield return new WaitForSeconds(5f);//Pause 5 seconds before making object flash again

        StartAnim();//Call method to repeat coroutine
    }

    /// <summary>
    /// If player enters the collider then
    /// inform GameManager to end level
    /// </summary>
    /// <param name="_other"></param>
    private void OnTriggerEnter(Collider _other)
    {
        if(_other.tag == "Player")
        {           
            GameManager.PlayerReachedEndLevel();
        }       
    }
}
