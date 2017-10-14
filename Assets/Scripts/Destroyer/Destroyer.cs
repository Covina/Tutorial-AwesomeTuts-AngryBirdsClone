using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    // Detect collisions with the shredder
    void OnTriggerEnter2D(Collider2D target)
    {
        // if its any of these, desotry them
        if(target.tag == "Bird" || target.tag == "Pig" || target.tag == "Brick")
        {
            Destroy(target.gameObject);
        }

    }

}
