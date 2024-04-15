using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // you have one job
        // destroy the explosion animation after x seconds
        Destroy(this.gameObject, 3f);
        
    }

}
