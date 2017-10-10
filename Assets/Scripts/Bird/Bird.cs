using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {


    public BirdState birdState { set; get; }

    // Trail Renderer
    //private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;

    // Rigid body
    private Rigidbody2D rigidBody;

    // Circle collider
    private CircleCollider2D circleCollider;

    // Audio Source
    private AudioSource audioSource;
       



	// Use this for initialization
	void Awake () {

        InitializeVariables();
		
	}
	
    // Last thing to run after physics have settled
    void FixedUpdate()
    {
        // Did we throw the bird and its velocity has dropped below our min threshold
        if(birdState == BirdState.Thrown && rigidBody.velocity.sqrMagnitude <= GameVariables.MinVelocity)
        {
            // bird has slowed so much, its time to destroy it
            StartCoroutine(DestroyAfterDelay(2f));

        }

    }

    // =======


    // init all vars
    private void InitializeVariables()
    {
        // Set all the references
        trailRenderer = GetComponent<TrailRenderer>();

        //
        rigidBody = GetComponent<Rigidbody2D>();

        //
        circleCollider = GetComponent<CircleCollider2D>();

        //
        audioSource = GetComponent<AudioSource>();


        // Disable the trail renderer for now
        trailRenderer.enabled = false;
        trailRenderer.sortingLayerName = "Foreground";


        // Disable physics for now
        rigidBody.isKinematic = true;

        // Make touch input easier at start
        circleCollider.radius = GameVariables.BirdColliderRadiusBig;


        // Set initial bird state
        birdState = BirdState.BeforeThrown;

    }


    // When Bird is thrown
    public void OnThrow()
    {
        // Play the attached sound when the bird is thrown
        audioSource.Play();

        // Turn on the Trail renderer
        trailRenderer.enabled = true;

        // Turn on physics
        rigidBody.isKinematic = false;

        // Set collider back to normal
        circleCollider.radius = GameVariables.BirdColliderRadiusNormal;

        // Update bird state to THROWN
        birdState = BirdState.Thrown;

    }


    // Wait to destroy the bird, called if bird velocity is too slow
    IEnumerator DestroyAfterDelay(float delay)
    {
        // wait some amount of time
        yield return new WaitForSeconds(delay);

        // destroy the bird
        Destroy(gameObject);
    }




}
