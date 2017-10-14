using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour {

    // how much health does the pig have
    public float health = 150f;

    // what sprite image do we change to when the pig is damaged
    public Sprite spriteShownWhenHurt;

    // sound to play when hit
    private AudioSource audioSource;

    //
    private float changeSpriteHealth;

	// Use this for initialization
	void Start () {

        audioSource = GetComponent<AudioSource>();

        changeSpriteHealth = health - 30f;


	}
	

    void OnCollisionEnter2D(Collision2D target)
    {
        // if the collider doesnt have a rigidbody, exit
        if(target.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }

        // if the BIRD is the the collider
        if(target.gameObject.tag == "Bird")
        {
            // play the sound
            audioSource.Play();

            // destroy the pig
            Destroy(gameObject);

        } else
        {
            // its something else

            // Get the velocity of the colliding object to caluclate damage
            float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10f;

            // if the damage number is greater then 10, trigger the sound effect
            if(damage >= 10)
            {
                audioSource.Play();
            }

            // Apply the damage
            health -= damage;

            // If we are below the threshold, change the image to the Hurt version
            if(health < changeSpriteHealth)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteShownWhenHurt;
            }

            // Is the pig still alive?
            if(health <= 0)
            {
                Destroy(gameObject);
            }


        }

    }


	// Update is called once per frame
	void Update () {
		
	}
}
