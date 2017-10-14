using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //
    public CameraFollow cameraFollow;

    // 
    public Slingshot slingShot;

    // 
    private int currentBirdIndex;

    // 
    [HideInInspector]
    public static GameState gameState;

    // store the game objects
    private List<GameObject> bricks;
    private List<GameObject> birds;
    private List<GameObject> pigs;

    // Use this for initialization
    void Awake () {

        //
        gameState = GameState.Start;

        // disable when game begins
        slingShot.enabled = false;

        // Populate the lists
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));

        // disable the sling shot bands
        slingShot.slingShotLineRenderer1.enabled = false;
        slingShot.slingShotLineRenderer2.enabled = false;

    }
	

    void OnEnable()
    {
        // subscribe to the event
        slingShot.birdThrown += SlingShotBirdThrown;
    }


    void OnDisable()
    {
        // unsubscribe to the event
        slingShot.birdThrown -= SlingShotBirdThrown;

    }

    // Update is called once per frame
    void Update () {

        // Check in on the game state
        switch (gameState)
        {

            case GameState.Start:

                if (Input.GetMouseButtonUp(0))
                {
                    AnimateBirdToSlingshot();
                }

                break;

            case GameState.Playing:

                // we threw the bird and either 1) objects are destroyed or 2) enough time has passsed
                if (slingShot.slingShotState == SlingShotState.BirdFlying && (BricksBirdsPigsStoppedMoving() || Time.time - slingShot.timeSinceThrown > 5f))
                {
                    slingShot.enabled = false;

                    // disable the sling shot bands
                    slingShot.slingShotLineRenderer1.enabled = false;
                    slingShot.slingShotLineRenderer2.enabled = false;


                    AnimateCameraToStartPosition();

                    gameState = GameState.BirdMovingToSlingshot;
                }

                break;

            case GameState.Won:
            case GameState.Lost:

                // if player touches the screen, reload the scene
                if (Input.GetMouseButtonDown(0))
                {
                    // reload same scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                break;

        }

	} // Update()

    // move bird from ground to slingshot
    private void AnimateBirdToSlingshot()
    {
        // change gamestate
        gameState = GameState.BirdMovingToSlingshot;

        // animate bird from starting location to ready position
        birds[currentBirdIndex].transform.positionTo(
            // float duration
            Vector2.Distance(birds[currentBirdIndex].transform.position / 10, slingShot.birdWaitPosition.position) / 10,
            // end value 
            slingShot.birdWaitPosition.position
            ).setOnCompleteHandler((x) =>
           {
               x.complete();
               x.destroy();

               gameState = GameState.Playing;

               // enable the slingshot
               slingShot.enabled = true;

               // disable the sling shot bands
               slingShot.slingShotLineRenderer1.enabled = false;
               slingShot.slingShotLineRenderer2.enabled = false;


               // assign which bird game object
               slingShot.birdToThrow = birds[currentBirdIndex];
           });

            
    }

    /// <summary>
    /// See if all the objects has resolved
    /// </summary>
    /// <returns></returns>
    private bool BricksBirdsPigsStoppedMoving()
    {
        // loop through all items in all three lists
        foreach(var item in bricks.Union(birds).Union(pigs))
        {
            // do the items exist and are they moving faster than min velocity?
            if(item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > GameVariables.MinVelocity)
            {
                return false;
            }

        }

        // 
        return true;
    }

    /// <summary>
    /// Check if all pigs are detroyed
    /// </summary>
    /// <returns></returns>
    private bool AllPigsDestroyed ()
    {
        // return all pigs and check if they are all null
        return pigs.All(x => x == null);
    }


    /// <summary>
    /// Move the camera back to the starting position
    /// </summary>
    private void AnimateCameraToStartPosition()
    {
        // calculate time duration
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.startingPosition) / 10;

        if(duration == 0.0f)
        {
            duration = 0.1f;
        }

        // move the camera
        Camera.main.transform.positionTo(duration, cameraFollow.startingPosition).
            setOnCompleteHandler((x) =>
            {
                // stop the camera
                cameraFollow.isFollowing = false;

                // check if all pigs are destroyed and level was won
                if (AllPigsDestroyed() == true)
                {
                    // congrats!
                    gameState = GameState.Won;
                }
                else if (currentBirdIndex == birds.Count - 1)
                {
                    // ran out of birds
                    gameState = GameState.Lost;

                } else
                {
                    // game continues...

                    // set Slingshot back to idle in prep for next shot
                    slingShot.slingShotState = SlingShotState.Idle;

                    // increment to next bird
                    currentBirdIndex++;

                    // Load up next bird
                    AnimateBirdToSlingshot();
                }

            });

    }

    /// <summary>
    /// Things to do when we know that the bird has been thrown
    /// </summary>
    private void SlingShotBirdThrown()
    {
        // assign which bird game object to follow
        cameraFollow.birdToFollow = birds[currentBirdIndex].transform;

        // turn on the follow
        cameraFollow.isFollowing = true;

        // Disable the trajectory lines
        slingShot.SetTrajectoryLineRendererActive(false);

    }


}
