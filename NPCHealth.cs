using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;
    float firstKnockPercent = .66f;
    float secondKnockPercent = .33f;
    bool firstKnock = false;
    bool secondKnock = false;
    public Vector2 knockbackDirection = new Vector2(1, 1); // Default to the right and back
    public float knockbackStrength = 5f;
    private Rigidbody2D rb;
    [SerializeField] bool isEnemy;
    [SerializeField] int manaReward = 5;
    [SerializeField] int scoreReward;
    [SerializeField] int goldReward; 
    private Animator animator;
    GameObject levelManager;
    LevelManager LevManScript; 

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        levelManager = GameObject.FindGameObjectWithTag("Level Manager");
        LevManScript = levelManager.GetComponent<LevelManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (!firstKnock && currentHealth < (maxHealth * firstKnockPercent))
        {
            ApplyKnockback();
            PlayTookDamageAnimation();
            firstKnock = true;
        }
        else if (!secondKnock && currentHealth < (maxHealth * secondKnockPercent))
        {
            ApplyKnockback();
            PlayTookDamageAnimation();
            secondKnock = true; 
        }
    }

    void PlayTookDamageAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("TookDamage");
        }
    }

    private void ApplyKnockback()
    {
        if (rb != null)
        {
            rb.AddForce(knockbackDirection.normalized * knockbackStrength, ForceMode2D.Impulse);
        }
    }

    private void Die()
    {
        //add animation
        if (isEnemy)
        {
            LevManScript.PlayerTower.RegenerateMana(manaReward);
            LevManScript.AddScore(scoreReward); // 1 point per enemy kill
            LevManScript.AddCoins(goldReward); // 10 gold per enemy kill
        }
        else
        {
           LevManScript.EnemyTower.RegenerateMana(manaReward);
        }
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        // Optional: Trigger any healing effects/animations
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}
