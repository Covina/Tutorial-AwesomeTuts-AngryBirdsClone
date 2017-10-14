using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    /// <summary>
    /// Load the Level Select menu
    /// </summary>
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelMenu");
    }



}
