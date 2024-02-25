using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAlly : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int attackDamage = 5;
    public Transform enemyTower; // Assign via Inspector
    public float attackRange = .5f; // Attack range
    public LayerMask enemyLayer; // Assign in Inspector
    private Rigidbody2D rb;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;
    public Collider2D attackTrigger;
    private bool shouldMove = true;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private bool towerInRange = false;
    GameObject actualTower;
    private Animator animator;
    [SerializeField] bool isMultiTarget = false;
    [SerializeField] bool isPierce = false;
    [SerializeField] int pierceAmount = 0; 

    void Start()
    {
        actualTower = GameObject.FindGameObjectWithTag("Enemy Tower");
        enemyTower = actualTower.transform;
        rb = GetComponent<Rigidbody2D>();
        attackTrigger.enabled = true; // Enable attack trigger by default
        animator = GetComponent<Animator>();
    }

    void PlayWalkAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", shouldMove);
        }
    }

    void PlayAttackAnimation()
    {
        Debug.Log("Attack animation entered");
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    void PlayTookDamageAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("TookDamage");
        }
    }

    void Update()
    {
        if (shouldMove && !towerInRange)
        {
            MoveTowardsEnemyTower();
        }
        
        TryAttackEnemiesInRange();
        PlayWalkAnimation();

    }

    void TryAttackEnemiesInRange()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            //Debug.Log("This is enemies in range: " + enemiesInRange.Count);
            if (enemiesInRange.Count > 0)
            {
                if (!isMultiTarget && !isPierce)
                {
                    //Debug.Log("Enemy hit at this time: " + Time.time);
                    AttackEnemy(enemiesInRange[0]); // Attack the first enemy in range
                    lastAttackTime = Time.time;
                }
                else if (isMultiTarget)
                {
                    for (int i = 0; i < enemiesInRange.Count; i++)
                    {
                        Debug.Log("enemy in range");
                        AttackEnemy(enemiesInRange[i]);
                    }
                    if (towerInRange)
                    {
                        AttackEnemyTower(actualTower);
                    }
                    lastAttackTime = Time.time;

                }
                else
                {
                    bool towerPierced = false;
                    if (enemiesInRange.Count < pierceAmount && towerInRange)
                    {
                        AttackEnemyTower(actualTower);
                        pierceAmount--;
                        towerPierced = true;
                    }
                    for (int i = 0; i < Mathf.Min(enemiesInRange.Count, pierceAmount); i++)
                    {
                        AttackEnemy(enemiesInRange[i]);
                    }
                    if (towerPierced)
                    {
                        pierceAmount++;
                    }
                    lastAttackTime = Time.time;
                }
                
            }
            else if (towerInRange)
            {
                AttackEnemyTower(actualTower); // Attack the tower if no enemies are in range
                lastAttackTime = Time.time;
            }
            
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("This is what ally hit +" + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collider.gameObject);
            shouldMove = false;
        }
        else if (collider.gameObject.CompareTag("Enemy Tower"))
        {
            towerInRange = true; 
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collider.gameObject);
            if (enemiesInRange.Count == 0)
            {
                shouldMove = true;
            }
        }
        else if (collider.gameObject.CompareTag("Enemy Tower"))
        {
            towerInRange = false; 
        }
    }

    void MoveTowardsEnemyTower()
    {
        Vector2 targetPosition = new Vector2(enemyTower.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
    }

    void AttackEnemy(GameObject enemy)
    {
        NPCHealth enemyHealth = enemy.GetComponent<NPCHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
            PlayAttackAnimation();
            //StartCoroutine(AttackCooldown());
        }
    }

    void AttackEnemyTower(GameObject tower)
    {
        Tower enemyTower = tower.GetComponent<Tower>();
        if (enemyTower != null)
        {
            enemyTower.TakeDamage(attackDamage);
            PlayAttackAnimation();
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

    void OnDrawGizmos()
    {
        // Visualize the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
