using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        try
        {
            if(_other.tag == "Player")
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                Destroy(_other.gameObject);
            }

        }
        catch
        {
            Debug.Log("This is not a gameobject, or something is very wrong");
        }
    }
}
