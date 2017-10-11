using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {


    private Vector3 slingShotMiddleVector;

    [HideInInspector]
    public SlingShotState slingShotState;


    // The tops of the two arms of the slingshot
    public Transform slingShotLeftOrigin, slingShotRightOrigin;


    //
    public LineRenderer slingShotLineRenderer1;
    public LineRenderer slingShotLineRenderer2;
    public LineRenderer slingShotTrajectoryLineRenderer;

    // Bird to throw
    [HideInInspector]
    public GameObject birdToThrow;

    // 
    public Transform birdWaitPosition;

    //
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


        // Set the line renderer positions
        slingShotLineRenderer1.SetPosition(0, slingShotLeftOrigin.position);    // left
        slingShotLineRenderer2.SetPosition(0, slingShotRightOrigin.position);   // right


        // Calculate the middle vector between the wishbone
        slingShotMiddleVector = new Vector3( (slingShotLeftOrigin.position.x + slingShotRightOrigin.position.x) / 2,  (slingShotLeftOrigin.position.y + slingShotRightOrigin.position.y) / 2, 0);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    private void InitializeBird()
    {

    }

} // Slingshot
