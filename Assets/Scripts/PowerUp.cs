using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]  // 0 = Triple SHot, 1 = Speed, 2 = Shields
    private int powerupID;

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
          
            if (player != null)
            {
                switch(powerupID)
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
                    default: // Default value
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
        
}
