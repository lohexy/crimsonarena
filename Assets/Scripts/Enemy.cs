using UnityEngine;



public class Enemy : MonoBehaviour
{

        [Header("Префаби Луту")]
    public GameObject xpPrefab;
    public GameObject bronzeCoinPrefab;
    public GameObject silverCoinPrefab;
    public GameObject goldCoinPrefab;

    [Header("Шанси випадіння грошей")]
    [Range(0f, 100f)] public float moneyDropChance = 50f;
    [Range(0f, 100f)] public float goldChance = 5f;
    [Range(0f, 100f)] public float silverChance = 25f;

    [Header("Налаштування Атаки")]
public int attackDamage = 10;     
public float attackCooldown = 1.5f;
private float nextAttackTime = 0f;

    public float speed = 2.5f;
    public int maxHealth = 3;
    
    private int currentHealth;
    private Transform playerTransform;

    void Start()
    {
        currentHealth = maxHealth;

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
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
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