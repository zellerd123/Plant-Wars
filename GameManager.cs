using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int coins { get; private set; }
    public int playerScore { get; private set; }
    public int playerLevel { get; private set; }
   


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


   

    public int AddCoins(int amount)
    {
        coins += amount;
        return coins; 
    }

    public int AddScore(int amount)
    {
        playerScore += amount;
        return playerScore; 
    }

    public int SetLevel(int level)
    {
        playerLevel = level;
        return playerLevel;
    }


    // Other methods as needed
}

