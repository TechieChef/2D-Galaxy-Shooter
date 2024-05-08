using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
       
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // get audio source component - makes it more flexible in future
        _audioSource = GetComponent<AudioSource>();
       
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

        // mitigate the annoying "variable is assigned but never used" warning
        _ = _isSpeedBoostActive == false;
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
           
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

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
    }


    public void Damage()
    {
        // if shields is active
        // do nothing
        // use return; keyword
        // deactivate shields and resume damage
        if(_isShieldBoostActive == true)
        {
            _isShieldBoostActive = false;
            // disable visualizer
            _shieldVisualizer.SetActive(false);
            return;
        }

        // if Enemy Laser hits Player, only take one life
    
        _lives--;
        // call updatelives method
        _uiManager.UpdateLives(_lives);

        // if lives = 2 enable left engine
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        // if lives = 1 enable right  engine
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath(); 

            Destroy(this.gameObject);
        }

        Debug.Log($"Lives: {_lives}");
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
    {
        _isShieldBoostActive = true;
        // enable visualizer
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
