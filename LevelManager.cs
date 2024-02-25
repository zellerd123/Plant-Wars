using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private bool isPaused = false;
    public Tower PlayerTower; // Assign in the Inspector
    public Tower EnemyTower; // Assign in the Inspector
    public TMP_Text coinText;
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text manaText;
    public int coins;
    public int playerScore;
    public int playerLevel;
    public GameObject pauseMenu;
    [SerializeField] CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCoins(int amount)
    {
        coins = GameManager.Instance.AddCoins(amount);
        UpdateCoinUI();
    }

    public void AddScore(int amount)
    {
        playerScore = GameManager.Instance.AddScore(amount);
        UpdateScoreUI();
    }

    public void SetLevel(int level)
    {
        playerLevel = GameManager.Instance.SetLevel(level);
        UpdateLevelUI();
    }


    // UI update methods
    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + playerScore;
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Level: " + playerLevel;
    }
    

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        

        // Optionally, toggle the visibility of a pause menu
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }
        if (canvasGroup != null)
        {
            canvasGroup.interactable = !isPaused; // Disable buttons when paused
            canvasGroup.blocksRaycasts = !isPaused; // Prevent click events when paused
        }

        // Handle any other pause-related functionality
    }
}
