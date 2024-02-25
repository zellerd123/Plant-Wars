using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LossUI : MonoBehaviour
{
    public void openMainMenu()
    {
        // Load the first level or game scene
        SceneManager.LoadScene("MainMenu"); // Replace with your game scene name
    }

    public void openMainGame()
    {
        // Load the first level or game scene
        SceneManager.LoadScene("MainGame"); // Replace with your game scene name
    }
}
