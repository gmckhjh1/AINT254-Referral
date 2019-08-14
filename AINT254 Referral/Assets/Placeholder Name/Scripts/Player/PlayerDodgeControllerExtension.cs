using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeControllerExtension : MonoBehaviour
{
    //Note: all rigidbody.drag forces are hardcoded to maintain
    //consistency with the RBFPController.

    [SerializeField] public AnimationCurve jumpX, jumpY;
    private Vector3 dodgeLeft;
    private Vector3 dodgeRight;
    [SerializeField] float dodgeForce = 10.0f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] private Animator dodgeAnimator;
    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        //dodgeAnimator = GetComponent<Animator>();
        dodgeAnimator.enabled = false;
    }
          
    /// <summary>
    /// Call appropriate dodge method based on parameter
    /// passed from calling class.
    /// </summary>
    /// <param name="dodgeType"></param>
    public void StartDodge(String dodgeType)
    {
        try
        {
            switch (dodgeType)
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
        body.drag = 0f;
        body.AddRelativeForce(new Vector3(dodgeForce, 0f, 0f), ForceMode.Impulse);
        body.drag = 5f;
    }

    /// <summary>
    ///  Add force to the rigidbody on the z axis
    /// to move player rapidly to the left.
    /// </summary>
    private void DodgeLeft()
    {
        body.drag = 0f;
        body.AddRelativeForce(new Vector3(-dodgeForce, 0f, 0f), ForceMode.Impulse);
        body.drag = 5f;
    }

    /// <summary>
    /// Make the player leap forward to avoid any gap in terrain.
    /// </summary>
    private void Leap()
    {
        body.drag = 0f;
        body.AddRelativeForce(new Vector3(0f, jumpForce, (dodgeForce / 4)), ForceMode.Impulse);
        body.drag = 5f;
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
