using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    // begin ammo count variables
    [SerializeField]
    private TMP_Text _ammoValueText;
    // end ammo count variables
    // game over text
    [SerializeField]
    private TMP_Text _gameOverText;
    // restart level text
    [SerializeField]
    private TMP_Text _restartText;
    // variable to handle calling GameManager script
    private GameManager _gameManager;



    // Start is called before the first frame update
    void Start()
    {
        // assign text component to the handle
        _scoreText.text = "Score: " + 0;

        // ensure Game Over Text is off
        _gameOverText.gameObject.SetActive(false);

        // call the game manager
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        // null check notify for game manager
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
        
    }

    // method to update the score
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    // method to update lives
    public void UpdateLives(int currentLives)
    {
        // display img sprite
        // give it a new one based on the currentLives index
        _LivesImg.sprite = _liveSprites[currentLives];

        // if currentlives is 0 turn on gameover text
        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }

    // method to update ammo count
    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoValueText.text = ammoCount.ToString();
    }

    public void GameOverSequence()
    {
        // access the game manager
        _gameManager.GameOver();

        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    // Game Over flicker
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
