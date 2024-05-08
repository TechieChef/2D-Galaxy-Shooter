using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
  
{
    [SerializeField]
    private float _speed = 4.0f;
    // variable to describe the Enemy Lasers
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    // variable to handle the firing rate of Enemyy Laser
    private float _fireRate = 3.0f;
    // variable that tells game Enemy can Fire
    private float _canFire = -1;
    // variable to prevent ghost firing from Enemy
    private bool _isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
            Debug.Log("Enemy could not find the player.");


        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // check to see if Time.time is greater than -1 
        // This value is greater only if the game is actively running
        if (Time.time > _canFire && _isAlive == true)
        {
            // set the firing rate range for a randome value between 3 and 7 seconds, float
            _fireRate = Random.Range(3.0f, 7.0f);
            // set up firing with time value
            _canFire = Time.time + _fireRate;
            // instantiate (spawn) Laser Prefab
            // place instantiation within a gameObject for script communication
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            enemyLaser.GetComponent<Laser>().AssignEnemyLaser();
            // Laser array
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _isAlive = false;
            _player.Damage();

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Laser"))
        {
            _isAlive = false;
            Destroy(other.gameObject); 
   
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            // Bugfix: Remove Laser Shot sound after Enemy Destroyed
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
