using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldBoostActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    // begin shield strength variables
    [SerializeField]
    private GameObject _shieldStrengthActive;
    [SerializeField]
    private TMP_Text _shieldStrengthTextActive;
    [SerializeField]
    private TMP_Text _shieldStrengthLabelActive;
    [SerializeField]
    private int _shieldStrength = 0;
    // end shield strength variables
    [SerializeField]
    private GameObject _leftEngine; 
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    // begin ammo count variables
    [SerializeField]
    private TMP_Text _ammoValueText;
    [SerializeField]
    private int _ammoCount = 15;
    // end ammo count variables

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
               
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        // establish ammo count text
        _ammoValueText.text = _ammoCount.ToString();

        // get audio source component - makes it more flexible in future
        _audioSource = GetComponent<AudioSource>();

        // consolidate null checks within own method
        DoNullChecks();

        // mitigate the annoying "variable is assigned but never used" warning
        _ = _isSpeedBoostActive == false;
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // update to include ammo count is greater than 0
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
        {
            FireLaser();
           
        }

    }

    private void DoNullChecks()
    {

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        // null check audio source and play sound if not null
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        // null check shield strength
        if (_shieldStrengthActive == null)
        {
            Debug.LogError("Shield Strength Visual is NULL.");
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // increase speed when shift key is pressed
        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        // play the laser audio clip
        _audioSource.Play();

        // reduce ammo by 1
        _ammoCount -= 1;
        // communicate with UIManager to update ammo
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    public void Damage()
    {
        // if Enemy Laser hits Player AND Shields inactive, only take one life
        if (_isShieldBoostActive == false)
        {
            _lives--;
            // call updatelives method
            _uiManager.UpdateLives(_lives);

            // if lives = 2 enable left engine
            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            // if lives = 1 enable right engine
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            if (_lives <= 0)
            {
                _spawnManager.OnPlayerDeath();

                Destroy(this.gameObject);
            }
        }
        // Debug.Log($"Lives: {_lives}");
        else
        {
            if (_shieldStrength > 0)
            {

                _shieldStrengthActive.SetActive(true);
                _shieldStrength -= 1;
                _shieldStrengthTextActive.text = _shieldStrength.ToString();
                _shieldStrengthLabelActive.enabled = true;

                if (_shieldStrength == 0)
                {
                    ShieldDeactivate();
                }

            }
        }
    }


    // deactivate shield strength visual
    public void ShieldDeactivate()
    {
        if (_isShieldBoostActive == true)
        {
            _isShieldBoostActive = false;
            _shieldStrengthActive.SetActive(false);
            _shieldVisualizer.SetActive(false);
        }

    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldBoostActive()
    // remove if for resetting shield
    {
        _isShieldBoostActive = true;
        _shieldStrengthActive.SetActive(true);
        _shieldStrength = 3;
        _shieldStrengthTextActive.text = _shieldStrength.ToString();
        _shieldStrengthLabelActive.enabled = true;
        _shieldVisualizer.SetActive(true);
        
    }

    // method to add 10 to the score
    // communicate with the UI to update the score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }
}
