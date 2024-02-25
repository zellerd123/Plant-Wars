using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMesh Pro for UI
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int maxMana = 100;
    private int currentMana;
    public int manaRegenerationRate = 1;
    public Image healthBar; // Green health bar
    public Image damageBar; // Red damage bar
    private int manaTowerUpgradeCost = 50;
    [SerializeField] int[] upgradeCosts;
    [SerializeField] int[] manaCapUpgrades;
    [SerializeField] int[] manaRegenInc; 
    private int currentUpgradeLevel = 0;



    public TextMeshProUGUI healthDisplay; // UI for displaying health
    public TextMeshProUGUI manaDisplay;   // UI for displaying mana

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = 0;
        UpdateHealthBar();
        UpdateManaDisplay();
        StartCoroutine(RegenerateMana());
    }

    public int getMana()
    {
        return currentMana;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below 0
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            // Handle the tower's destruction here

            if (gameObject.tag == "Player Tower")
            {
                SceneManager.LoadScene("Loss");
            }
            else if (gameObject.tag == "Enemy Tower")
            {
                SceneManager.LoadScene("Win");
            }
            Debug.Log("Tower Destroyed");
            Destroy(gameObject);
        }
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(.33f);
            if (currentMana < maxMana)
            {
                currentMana += manaRegenerationRate;
                UpdateManaDisplay();
            }
        }
    }

    public void UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateManaDisplay();
        }
    }

    public void RegenerateMana(int amount)
    {
        if (currentMana + amount > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += amount;
        }
        UpdateManaDisplay();
    }

    public void UpgradeManaTower()
    {
        if (currentUpgradeLevel < upgradeCosts.Length && currentMana >= upgradeCosts[currentUpgradeLevel])
        {
            currentMana -= upgradeCosts[currentUpgradeLevel];
            maxMana += manaCapUpgrades[currentUpgradeLevel]; // Increment the mana cap
            manaRegenerationRate += manaRegenInc[currentUpgradeLevel]; // Increment the mana gain rate
            currentUpgradeLevel++; // Increase cost for next upgrade

            // Update UI and other relevant game elements
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(healthPercent, 1, 1);
    }

    private void UpdateManaDisplay()
    {
        if (manaDisplay != null)
            manaDisplay.text = "Mana: " + currentMana + "/" + maxMana;
    }

    // Additional tower-specific methods can be added here
}
