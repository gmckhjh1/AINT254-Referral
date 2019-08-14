using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Destroy(other.gameObject);
        }
        catch
        {
            Debug.Log("This is not a gameobject, or something is very wrong");
        }
    }
}
