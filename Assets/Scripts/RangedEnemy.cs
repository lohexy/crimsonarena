using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Налаштування стрільби")]
    public GameObject projectilePrefab;
    public float attackRange = 7f;
    public float fireRate = 2f;
    public float projectileSpeed = 5f;

    private Transform player;
    private float shotTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            shotTimer += Time.deltaTime;

            if (shotTimer >= fireRate)
            {
                Shoot();
                shotTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, 4f);
    }
}