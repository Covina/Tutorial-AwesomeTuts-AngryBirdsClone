using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {


    public Slingshot slingShot;

    private float dragSpeed = 0.01f;

    private float timeDragStarted;

    private Vector3 previousPosition;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        // check if sling is idle and we're in an active turn
        if(slingShot.slingShotState == SlingShotState.Idle && GameManager.gameState == GameState.Playing)
        {

            // is the player touching the screen?
            if(Input.GetMouseButtonDown(0))
            {

                // track the amount of time player is touching the screen
                timeDragStarted = Time.time;

                // init drag speed at 0f
                dragSpeed = 0f;

                // set previous position to the current position
                previousPosition = Input.mousePosition;

            } else if (Input.GetMouseButton(0) && (Time.time - timeDragStarted) > 0.005f)
            {

                // current touch position
                Vector3 input = Input.mousePosition;

                // how far did we drag between frames
                float deltaX = (previousPosition.x - input.x) * dragSpeed;
                float deltaY = (previousPosition.y - input.y) * dragSpeed;

                // stay within the global X position limits
                float newX = Mathf.Clamp(transform.position.x + deltaX, 0, 14.0f);

                // stay within the global Y position limits
                float newY = Mathf.Clamp(transform.position.y + deltaY, 0, 0.0f);

                // set the camera's new position
                transform.position = new Vector3(newX, newY, transform.position.z);

                // set previous position to the current position
                previousPosition = input;

                // slowly increase drag to slow it down
                if(dragSpeed < 0.1f)
                {
                    dragSpeed += 0.002f;
                }
            }


        }

	}
}
