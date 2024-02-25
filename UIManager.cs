using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartLevel()
    {
        // Load the first level or game scene
        SceneManager.LoadScene("MainGame"); // Replace with your game scene name
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void OpenStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void OpenMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}
