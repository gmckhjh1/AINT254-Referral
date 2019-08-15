using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeControllerExtension : MonoBehaviour
{
    //Note: all rigidbody.drag forces are hardcoded to maintain
    //consistency with the RBFPController.

    [SerializeField] public AnimationCurve jumpX, jumpY;
    private Vector3 m_dodgeLeft;
    private Vector3 m_dodgeRight;
    [SerializeField] private float m_dodgeForce = 10.0f;
    [SerializeField] private float m_jumpForce = 10.0f;
    [SerializeField] public Animator dodgeAnimator;
    private Rigidbody m_body;

    private void Start()
    {
        m_body = GetComponent<Rigidbody>();
        //dodgeAnimator = GetComponent<Animator>();
        dodgeAnimator.enabled = false;
    }
          
    /// <summary>
    /// Call appropriate dodge method based on parameter
    /// passed from calling class.
    /// </summary>
    /// <param name="_dodgeType"></param>
    public void StartDodge(String _dodgeType)
    {
        try
        {
            switch (_dodgeType)
            {
                case "Left":
                    DodgeLeft();
                    break;

                case "Right":
                    DodgeRight();
                    break;

                case "Jump":
                    Leap();
                    break;

                case "Vault":
                    Vault();
                    break;

                case "Slide":
                    Slide();
                    break;

                default:
                    Debug.Log("Nothing selected");
                    break;
            }            
        }
        catch
        {
            Debug.Log("Couldn't find action, maybe incorrect string " +
                "value being passed from RBFPController");
        }
    }

    /// <summary>
    /// Add force to the rigidbody on the z axis
    /// to move player rapidly to the right.
    /// </summary>
    private void DodgeRight()
    {
        m_body.drag = 0f;
        m_body.AddRelativeForce(new Vector3(m_dodgeForce, 0f, 0f), ForceMode.Impulse);
        m_body.drag = 5f;
    }

    /// <summary>
    ///  Add force to the rigidbody on the z axis
    /// to move player rapidly to the left.
    /// </summary>
    private void DodgeLeft()
    {
        m_body.drag = 0f;
        m_body.AddRelativeForce(new Vector3(-m_dodgeForce, 0f, 0f), ForceMode.Impulse);
        m_body.drag = 5f;
    }

    /// <summary>
    /// Make the player leap forward to avoid any gap in terrain.
    /// </summary>
    private void Leap()
    {
        m_body.drag = 0f;
        m_body.AddRelativeForce(new Vector3(0f, m_jumpForce, (m_dodgeForce / 4)), ForceMode.Impulse);
        m_body.drag = 5f;
    }

    /// <summary>
    /// Call vault animation once to move player over objects
    /// </summary>
    /// <returns></returns>    
    public IEnumerator Vault()
    {
        /*
        dodgeAnimator.enabled = true;
        yield return new WaitForSeconds(.4f);        
        dodgeAnimator.enabled = false;
        */
        return null;
                
    }

    /// <summary>
    /// Play the slide animation to move player under objects
    /// </summary>
    public IEnumerator Slide()
    {




        return null;
    }
    
}
