using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MultiplayerManagerUI : MonoBehaviour
{
    public void openMainMenu()
    {
        // Load the first level or game scene
        SceneManager.LoadScene("MainMenu"); // Replace with your game scene name
    }
}
