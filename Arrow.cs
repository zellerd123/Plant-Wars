using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    private Rigidbody2D rb;

    private float launchAngle;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial rotation to match the launch angle
        float launchAngleDegrees = CalculateLaunchAngle();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, launchAngleDegrees));

        // Apply initial velocity based on the launch angle
        Vector2 launchDirection = new Vector2(Mathf.Cos(launchAngleDegrees * Mathf.Deg2Rad), Mathf.Sin(launchAngleDegrees * Mathf.Deg2Rad));
        rb.velocity = launchDirection * speed;
    }

    private void Update()
    {
        RotateArrow();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            NPCHealth npc = other.GetComponent<NPCHealth>();
            if (npc != null)
            {
                npc.TakeDamage(damage);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            // Handle collision with the ground
        }

        Destroy(gameObject); // Destroy the arrow after hitting any target
    }

    private float CalculateLaunchAngle()
    {
        // Calculate and return the launch angle in degrees
        // This might involve getting the angle from the bow or another component
        return 45f; // Placeholder, replace with actual calculation or parameter
    }

    void RotateArrow()
    {
        if (rb.velocity.sqrMagnitude > 0.01f) // Check to avoid calculation when velocity is very small
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Adding 180 degrees to flip the direction
        }
    }

    public void SetLaunchAngle(float angle)
    {
        launchAngle = angle;
    }
}

