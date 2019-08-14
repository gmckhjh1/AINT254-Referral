using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//Class to handle management of game
//
public static class GameManager
{
     

    // Start is called before the first frame update
    static void Start()
    {
        
    }

    // Update is called once per frame
    static void Update()
    {
        
    }

    
    /// <summary>
    /// Close the Application
    /// </summary>
    static void CloseGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    /// <summary>
    /// Receive notification that player has 
    /// reached the end level checkpoint
    /// </summary>
    public static void PlayerReachedEndLevel()
    {
        CloseGame();
    }

    public static void GetSpawner()
    {

    }
}
