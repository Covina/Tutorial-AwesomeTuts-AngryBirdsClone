using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour {

    // How much do we want to move it?
    public float parallaxFactor;

    // storing previous amera position to determine movement delta
    private Vector3 previousCameraPosition;


	// Use this for initialization
	void Start () {

        // store the current camera position
        previousCameraPosition = Camera.main.transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {

        // calculate the distance on the X
        Vector3 delta = Camera.main.transform.position - previousCameraPosition;

        // Zero our the Y and Z
        delta.y = 0.0f;
        delta.z = 0.0f;
        
        // set the new transform position
        transform.position += delta / parallaxFactor;

        // store new position as previous
        previousCameraPosition = Camera.main.transform.position;
	}
}
