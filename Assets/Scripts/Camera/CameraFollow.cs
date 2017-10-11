using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [HideInInspector]
    public Vector3 startingPosition;

    [HideInInspector]
    public bool isFollowing;

    // Which bird are we following?
    [HideInInspector]
    public Transform birdToFollow;

    // Min camera X position (the start)
    private float minCameraX = 0.0f;

    // Max camera position (the end)
    private float maxCameraX = 14f;




	// 
	void Awake () {

        // get camera starting position
        startingPosition = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		
        // are we in the following state?
        if(isFollowing == true)
        {
            // do we have a bird to follow
            if(birdToFollow != null)
            {
                // get bird position
                var birdPosition = birdToFollow.position;

                // Clamp the X pos value to make sure bird position can't take camera beyond its bounds
                float x = Mathf.Clamp(birdPosition.x, minCameraX, maxCameraX);

                // Move camera
                transform.position = new Vector3(x, startingPosition.y, startingPosition.z);
            }
            else
            {
                // if the bird no longer exists (destroyed), stop following
                isFollowing = false;
            }

        }

	} // Update()
}
