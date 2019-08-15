using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//Class to handle all UI data
//
public class UIManager : MonoBehaviour
{   

    [SerializeField] private float m_timer = 60.0f;

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0)
        {
            GameManager.PlayerReachedEndLevel();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 70, 100), "Time left:");
        GUI.Label(new Rect(80, 10, 100, 100), m_timer.ToString("0"));
    }
}
