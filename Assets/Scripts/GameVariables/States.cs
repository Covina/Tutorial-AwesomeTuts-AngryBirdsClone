using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Slingshot
public enum SlingShotState { 

    Idle,           // Resting
    UserPulling,    // User is actively targeting
    BirdFlying      // Bird was released

}

// Game states
public enum GameState
{

    Start,                    
    BirdMovingToSlingshot,
    Playing,
    Won,
    Lost

}

// Bird state
public enum BirdState
{
    BeforeThrown,
    Thrown
}

