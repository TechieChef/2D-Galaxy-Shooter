using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]  // 0 = Triple SHot, 1 = Speed, 2 = Shields
    private int _powerupID;

    // audio clip for powerups
    [SerializeField]
    private AudioClip _clip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            // play audio at this point in script
            AudioSource.PlayClipAtPoint(_clip, transform.position);
          
            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0: // Triple Shot
                        player.TripleShotActive();
                        break;
                    case 1: // Speed Boost
                        // Debug.Log("Warp Speed!");
                        player.SpeedBoostActive();
                        break;
                    case 2: // Shields
                        // Debug.Log("Shields Up!");
                        player.ShieldBoostActive();
                        break;
                    case 3: // Ammo
                        // Debug.Log("Ammo Collected");
                        player.AmmoCollected();
                        break;
                    case 4: // Health
                        // Debug.Log("Add Health");
                        player.AddHealth();
                        break;
                    default: // Default value
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
        
}
