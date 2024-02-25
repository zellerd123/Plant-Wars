using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZone : MonoBehaviour
{

    
    public float healAmountPerSecond = 5f;
    private float accumulatedHeal = 0f;
    private bool isPlayerInZone = false;
    private Player playerScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<Player>();
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    private void Update()
    {
        if (isPlayerInZone && playerScript != null)
        {
            accumulatedHeal += healAmountPerSecond * Time.deltaTime;

            while (accumulatedHeal >= 1f)
            {
                playerScript.Heal(1);
                accumulatedHeal -= 1f;
            }
        }
    }
}