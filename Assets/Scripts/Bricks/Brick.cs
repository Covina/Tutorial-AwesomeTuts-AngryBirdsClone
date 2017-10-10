using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    // Audio
    private AudioSource audioSource;

    // Brick health
    public float health = 70f;


    void Awake()
    {
        // get the audio source
        audioSource = GetComponent<AudioSource>();
    }

    // did we collide with something
    void OnCollisionEnter2D(Collision2D target)
    {

        // check if it has a rigid body
        if(target.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            // Exit
            return;
        }

        // Calculate damage based on force
        float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10f;

        // Threshold of damage for playing the sound
        if(damage > 15)
        {
            // play the smash sound
            audioSource.Play();
        }

        // Apply the damage to health
        health -= damage;

        // Check if the brick is destroyed
        if(health <= 0)
        {
            Destroy(gameObject);
        }





    }


}
