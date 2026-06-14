using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            
            if (playerStats == null)
            {
                playerStats = collision.GetComponentInParent<PlayerStats>();
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            
            if (playerStats == null)
            {
                playerStats = collision.gameObject.GetComponentInParent<PlayerStats>();
            }

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}