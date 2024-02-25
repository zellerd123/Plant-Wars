using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.7f;
    public int attackDamage = 10;
    private float swingDelay = 0.2f;
    public float swingCooldown = 2f;
    private bool canSwing = true;
    public int maxHealth = 100;
    private int currentHealth;
    private bool towerHit = false;
    public float knockbackStrengthX = 10f;  // Horizontal knockback strength
    public float knockbackStrengthY = 4f;  // Vertical knockback strength



    private bool facingRight = true;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    public Collider2D attackCollider;
    public Animator swordAnimator;
    private Vector2 attackDirection;
    private float movement;
    public TextMeshProUGUI playerHealth;
    private HashSet<GameObject> enemiesHitThisSwing;


    void Start()
    {
        attackCollider.enabled = false; 
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        updateHealthDisplay();
        enemiesHitThisSwing = new HashSet<GameObject>();
        towerHit = false;
    }

    void Update()
    {
        HandleInput();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
      
    }

    void HandleInput()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (movement > 0 && !facingRight) Flip();
        else if (movement < 0 && facingRight) Flip();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.R) && canSwing) PerformAttack();
    }

    void HandleMovement()
    {
        Vector2 targetVelocity = new Vector2(movement * speed, rb.velocity.y);
        rb.velocity = targetVelocity;
    }


    void PerformAttack()
    {
        // Enable the attack collider for a short duration
        swordAnimator.SetTrigger("Swing");
        canSwing = false;
        enemiesHitThisSwing.Clear(); // Clear the list at the start of each swing
        towerHit = false;
        StartCoroutine(AttackSequence());
    }

    IEnumerator AttackSequence()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(swingDelay);
        attackCollider.enabled = false;
        yield return new WaitForSeconds(swingCooldown); // Assuming swingCooldown is the time before the next attack can be initiated
        canSwing = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("This is what hit by player " + other.tag);
        if (other.CompareTag("Enemy") && attackCollider.enabled)
        {
            if (!enemiesHitThisSwing.Contains(other.gameObject))
            {
                enemiesHitThisSwing.Add(other.gameObject);
                NPCHealth enemyHealth = other.GetComponent<NPCHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
        
        else if (other.CompareTag("Enemy Tower") && attackCollider.enabled && !towerHit)
        {
            Tower enemyTower = other.GetComponent<Tower>();
            if (enemyTower != null)
            {
                enemyTower.TakeDamage(attackDamage);
                towerHit = true; 
            }
        }
    }


    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = groundCheckDistance + collider2D.bounds.extents.y;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        // Visualize the raycast
        Gizmos.color = Color.red;
        Vector2 position = transform.position;
        Gizmos.DrawLine(position, position + Vector2.down * (groundCheckDistance + (collider2D ? collider2D.bounds.extents.y : 0f)));
    }
   

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        updateHealthDisplay();

        // Knockback
        if (currentHealth < maxHealth / 2)
        {
            Vector2 knockbackDirection = new Vector2(-1, 1).normalized;  // Left and upward
            rb.AddForce(knockbackDirection * new Vector2(knockbackStrengthX, knockbackStrengthY), ForceMode2D.Impulse);

        }


        // Placeholder for sound effect
        PlayDamageSound();

        // Placeholder for visual effect
        FlashEffect();

        if (currentHealth <= 0)
        {
            Debug.Log("Player Defeated");
            // Placeholder for defeat condition
        }
    }

    private void PlayDamageSound()
    {
        // Implement sound effect logic
    }

    private void FlashEffect()
    {
        // Implement flashing effect logic
    }

    public void Heal(int amount)
    {
        Debug.Log("heal entered");
        Debug.Log("This is MM " + Mathf.Min(currentHealth + amount, maxHealth));
        Debug.Log("This is curr health" + currentHealth);
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        updateHealthDisplay();
    }

    public void updateHealthDisplay()
    {
        if (playerHealth != null)
            playerHealth.text = "Player Health: " + currentHealth + "/" + maxHealth;
    }
}
