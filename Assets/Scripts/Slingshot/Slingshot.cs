using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {


    private Vector3 slingShotMiddleVector;

    [HideInInspector]
    public SlingShotState slingShotState;


    // The tops of the two arms of the slingshot
    public Transform slingShotLeftOrigin, slingShotRightOrigin;


    // The sling shot bands
    public LineRenderer slingShotLineRenderer1;
    public LineRenderer slingShotLineRenderer2;

    // The trajectory flight path of bird
    public LineRenderer slingShotTrajectoryLineRenderer;

    // Bird to throw
    [HideInInspector]
    public GameObject birdToThrow;

    // Staging position for next bird
    public Transform birdWaitPosition;

    // Starting throw speed
    public float throwSpeed;

    [HideInInspector]
    public float timeSinceThrown;

    // 
    public delegate void BirdThrown();

    //
    public event BirdThrown birdThrown;


    // Use this for initialization
    void Awake () {

        // Set all the line renderers to the foreground
        slingShotLineRenderer1.sortingLayerName = "Foreground";
        slingShotLineRenderer2.sortingLayerName = "Foreground";
        slingShotTrajectoryLineRenderer.sortingLayerName = "Foreground";

        // set state
        slingShotState = SlingShotState.Idle;


        // Set the first line renderer positions (vertex)
        slingShotLineRenderer1.SetPosition(0, slingShotLeftOrigin.position);    // left
        slingShotLineRenderer2.SetPosition(0, slingShotRightOrigin.position);   // right


        // Calculate the middle vector between the wishbone
        slingShotMiddleVector = new Vector3( (slingShotLeftOrigin.position.x + slingShotRightOrigin.position.x) / 2,  (slingShotLeftOrigin.position.y + slingShotRightOrigin.position.y) / 2, 0);


    }
	
	// Update is called once per frame
	void Update () {
		
        switch(slingShotState)
        {
            case SlingShotState.Idle:

                InitializeBird();
                DisplaySlingshotLineRenderers();


                // if player is touching the screen
                if(Input.GetMouseButtonDown(0))
                {
                    // get coordinates for in-game to see if they are touching a bird
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
                    // check if the birds location overlaps with where the user is touching the screen
                    if (birdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        // user is pulling the slingshot
                        slingShotState = SlingShotState.UserPulling;
                    }

                }


                break;


            case SlingShotState.UserPulling:

                DisplaySlingshotLineRenderers();

                if(Input.GetMouseButton(0))
                {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0f;

                    // 
                    if (Vector3.Distance(location, slingShotMiddleVector) > 1.5f)
                    {
                        // implement a cap for how far back the bird can be drawn
                        var maxPosition = (location - slingShotMiddleVector).normalized * 1.5f + slingShotMiddleVector;

                        birdToThrow.transform.position = maxPosition;

                    } else {
                        // set location 
                        birdToThrow.transform.position = location;
                    }


                    // calculate distance between slingshot middle and bird
                    var distance = Vector3.Distance(slingShotMiddleVector, birdToThrow.transform.position);


                    // show the trajectory line when player has pulled back the bird
                    DisplayTrajectoryLineRenderer(distance);

                } else
                {

                    // display line renderers
                    DisplaySlingshotLineRenderers();

                    timeSinceThrown = Time.time;

                    // calculate distance between slingshot middle and bird 
                    float distance = Vector3.Distance(slingShotMiddleVector, birdToThrow.transform.position);

                    if(distance > 1)
                    {
                        SetSlingshotLineRenderersActive(false);

                        // bird was thrown
                        slingShotState = SlingShotState.BirdFlying;

                        // Throw the bird
                        ThrowBird(distance);
                    } else
                    {
                        // 
                        birdToThrow.transform.positionTo(distance / 10, birdWaitPosition.position);
                        InitializeBird();
                    }


                }

                break;

        }



	}

    /// <summary>
    /// 
    /// </summary>
    private void InitializeBird()
    {
        // move bid to staging area
        birdToThrow.transform.position = birdWaitPosition.position;

        // Put sling state into idle
        slingShotState = SlingShotState.Idle;

        // Enable the line renderers in prep for shot
        SetSlingshotLineRenderersActive(true);


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    private void SetSlingshotLineRenderersActive(bool active)
    {
        // set to true/false
        slingShotLineRenderer1.enabled = active;
        slingShotLineRenderer2.enabled = active;


    }


    /// <summary>
    /// Set the vertex for the line renderer
    /// </summary>
    private void DisplaySlingshotLineRenderers()
    {
        // Set the second (index 1) vertex for the waiting state
        slingShotLineRenderer1.SetPosition(1, birdToThrow.transform.position);

        slingShotLineRenderer2.SetPosition(1, birdToThrow.transform.position);

    }

    /// <summary>
    /// Set enabled status for trajectory line renderer
    /// </summary>
    /// <param name="active"></param>
    public void SetTrajectoryLineRendererActive(bool active)
    {
        slingShotTrajectoryLineRenderer.enabled = active;
    }

    /// <summary>
    /// Show the trajectory line on screen
    /// </summary>
    /// <param name="distance"></param>
    private void DisplayTrajectoryLineRenderer(float distance)
    {
        // Enable it
        SetTrajectoryLineRendererActive(true);

        // Calculate and display

        // velocity vector:  v3 distance between bird stage and its current position (player pulling slingshot)
        Vector3 vel2 = slingShotMiddleVector - birdToThrow.transform.position;

        int segmentCount = 15;

        // create array of vector2 coords
        Vector2[] segments = new Vector2[segmentCount];

        // set first element as starting loc for the bird to throw
        segments[0] = birdToThrow.transform.position;

        // how far the bird will go
        Vector2 segVelocity = new Vector2(vel2.x, vel2.y) * throwSpeed * distance;

        // loop through to fill segments, skipping [0] since we defined that above
        for(int i = 1; i < segmentCount; i++)
        {
            // calculate time between trajectory points
            float time = i * Time.fixedDeltaTime * 5f;

            // take starting location, then add to it the Distance physics formula:  (v * t ) + (1/2 at^2)
            segments[i] = segments[0] + (segVelocity * time) + (0.5f * Physics2D.gravity * Mathf.Pow(time, 2) ) ;

        }

        // Set number of vertex positions (using latest docs)
        slingShotTrajectoryLineRenderer.positionCount = segmentCount;


        // populate the vertex positions along the calculated trajectory path
        for (int i = 0; i < segmentCount; i++) {

            // add the vertex position to the line renderer
            slingShotTrajectoryLineRenderer.SetPosition(i, segments[i]);

        }


    }


    /// <summary>
    /// Throw bird, passing in the distance
    /// </summary>
    /// <param name="distance"></param>
    private void ThrowBird(float distance)
    {

        // calculate launch velocity
        Vector3 throwVelocity = slingShotMiddleVector - birdToThrow.transform.position;

        // Prep the bird for throwing
        birdToThrow.GetComponent<Bird>().OnThrow();

        // Apply the force to the rigidbody and throw the bird
        birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVelocity.x, throwVelocity.y) * throwSpeed * distance;

        // fire the event for all subscribed delegates
        if(birdThrown != null)
        {
            birdThrown();
        }


    }



} // Slingshot
