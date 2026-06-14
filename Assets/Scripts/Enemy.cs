using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Налаштування Боса")]
    public bool isBoss = false;
    public GameObject royalChestPrefab;

    [Header("Префаби Луту")]
    public GameObject xpPrefab;
    public GameObject bronzeCoinPrefab;
    public GameObject silverCoinPrefab;
    public GameObject goldCoinPrefab;

    [Header("Шанси випадіння грошей")]
    [Range(0f, 100f)] public float moneyDropChance = 50f;
    [Range(0f, 100f)] public float goldChance = 5f;
    [Range(0f, 100f)] public float silverChance = 25f;

    [Header("Налаштування Атаки Ближнього Бою")]
    public int attackDamage = 10;     
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Налаштування Дальньої Атаки (Для EnemyLong)")]
    public bool isRanged = false;
    public GameObject projectilePrefab;
    public float attackRange = 7f;
    public float fireRate = 2f;
    public float projectileSpeed = 5f;
    private float nextFireTime = 0f;

    [Header("Характеристики Ворога")]
    public float speed = 2.5f;
    public int maxHealth = 3;
    private int currentHealth;
    private Transform playerTransform;
    
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("PlayerSprite") ?? GameObject.Find("Player");
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning($"Ворог {gameObject.name} не зміг знайти гравця на сцені при спавні!");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            RotateTowardsPlayer();

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (isRanged)
            {
                if (distanceToPlayer > attackRange)
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                    
                    HandleRangedAttack();
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (rb != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            
            rb.linearVelocity = direction * speed; 
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void HandleRangedAttack()
    {
        if (Time.time >= nextFireTime && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            
            Collider2D enemyCollider = GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            
            if (enemyCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(enemyCollider, projectileCollider);
            }

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                projectileRb.linearVelocity = direction * projectileSpeed;
            }

            Destroy(projectile, 4f); 
            nextFireTime = Time.time + fireRate;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(attackDamage);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    void Die()
    {
        if (isBoss)
        {
            if (royalChestPrefab != null)
            {
                GameObject bossChest = Instantiate(royalChestPrefab, transform.position, Quaternion.identity);
                WorldChest chestScript = bossChest.GetComponent<WorldChest>();
                if (chestScript != null)
                {
                    chestScript.isRoyal = true;
                    
                    SpriteRenderer sr = bossChest.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.color = Color.yellow;
                }
            }
        }

        if (xpPrefab != null)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }

        if (Random.Range(0f, 100f) <= moneyDropChance)
        {
            GameObject coinToSpawn = bronzeCoinPrefab;
            float randomRoll = Random.Range(0f, 100f);

            if (randomRoll <= goldChance)
            {
                coinToSpawn = goldCoinPrefab;
            }
            else if (randomRoll <= goldChance + silverChance)
            {
                coinToSpawn = silverCoinPrefab; 
            }

            if (coinToSpawn != null)
            {
                Instantiate(coinToSpawn, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    
}