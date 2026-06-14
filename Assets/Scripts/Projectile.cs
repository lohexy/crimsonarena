using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2.5f;
    public int damage = 1;

    private Vector2 moveDirection;

    public void Setup(Vector2 direction)
    {
        moveDirection = direction;
        Destroy(gameObject, lifetime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.gameObject.name.Contains("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}