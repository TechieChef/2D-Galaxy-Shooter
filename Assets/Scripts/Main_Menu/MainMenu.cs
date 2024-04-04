using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        // load the game when button is pressed
        SceneManager.LoadScene(1); // int value of main game scene found in build settings
    }
}
