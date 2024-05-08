using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        // if the r key was pressed
        // restart the current scene
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            // Load the game scene by "name" or number found in File >> Build Settings
            SceneManager.LoadScene("Game"); // Current Game Scene
        }

        // if escape key is pressed
        // quit the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // quit the game
            Application.Quit();

        }
    }

    public void GameOver()
    {
        // ensure method is called
        // Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }
}
