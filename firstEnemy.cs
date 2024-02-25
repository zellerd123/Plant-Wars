using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int attackDamage = 5;
    public Transform playerTower; // Assign via Inspector
    public float attackRange = 0.5f; // Attack range
    public LayerMask playerLayer; // Assign in Inspector
    private Rigidbody2D rb;
    private GameObject actualPlayerTower;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;
    public Collider2D attackTrigger;
    private bool shouldMove = true;
    private List<GameObject> playersAndAlliesInRange = new List<GameObject>();
    private bool playerTowerInRange = false;
    GameObject player;

    void Start()
    {
        actualPlayerTower = GameObject.FindGameObjectWithTag("Player Tower");
        player = GameObject.FindGameObjectWithTag("Player");
        playerTower = actualPlayerTower.transform;
        rb = GetComponent<Rigidbody2D>();
        attackTrigger.enabled = true; // Enable attack trigger by default
    }

    void Update()
    {
        if (shouldMove)
        {
            MoveTowardsPlayerTower();
        }

        TryAttackPlayersAndAlliesInRange();
    }

    void TryAttackPlayersAndAlliesInRange()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            //Debug.Log("This is playersAndAlliesInRange.Count: " + playersAndAlliesInRange.Count);
            if (playersAndAlliesInRange.Count > 0)
            {
                AttackPlayerOrAlly(playersAndAlliesInRange[0]); // Attack the first player or ally in range
                lastAttackTime = Time.time;
            }
            else if (playerTowerInRange)
            {
                AttackPlayerTower(actualPlayerTower); // Attack the tower if no players or allies are in range
                lastAttackTime = Time.time;
            }
        }
    }

    void AttackPlayerOrAlly(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            AttackPlayer();
        }
        else
        {
            AttackAlly(target);
        }
    }

    void MoveTowardsPlayerTower()
    {
        if (playerTower != null)
        {
            Vector2 targetPosition = new Vector2(playerTower.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("This is enter tag:" + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Ally"))
        {
            playersAndAlliesInRange.Add(collider.gameObject);
            shouldMove = false;
        }
        else if (collider.gameObject.CompareTag("Player Tower"))
        {
            playerTowerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("This is exit tag:" + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Ally"))
        {
            playersAndAlliesInRange.Remove(collider.gameObject);
            if (playersAndAlliesInRange.Count == 0)
            {
                shouldMove = true;
            }
        }
        else if (collider.gameObject.CompareTag("Player Tower"))
        {
            playerTowerInRange = false;
        }
    }

    void AttackPlayer()
    {
        // Assuming the player has a script with a TakeDamage method
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
            //StartCoroutine(AttackCooldown());
        }
    }

    void AttackAlly(GameObject ally)
    {
        NPCHealth allyHealth = ally.GetComponent<NPCHealth>();
        if (allyHealth != null)
        {
            allyHealth.TakeDamage(attackDamage);
            //StartCoroutine(AttackCooldown());
        }
    }

    void AttackPlayerTower(GameObject tower)
    {
        Tower playerTower = tower.GetComponent<Tower>();
        if (playerTower != null)
        {
            playerTower.TakeDamage(attackDamage);
        }
        //StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        lastAttackTime = Time.time;
        attackTrigger.enabled = false; // Disable the attack trigger during cooldown
        yield return new WaitForSeconds(attackCooldown);
        attackTrigger.enabled = true; // Re-enable the attack trigger after cooldown
    }
}
