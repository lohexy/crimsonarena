using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName = "Меч";
    public float attackRate = 0.6f;
    public float attackRange = 1.5f;
    public int damage = 2;
    public LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private Vector2 lastMousePosition;

    public void Fire(Vector2 mousePosition) 
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            lastMousePosition = mousePosition;
            Attack();
        }
    }

    void Attack()
    {
        Vector2 direction = (lastMousePosition - (Vector2)transform.position).normalized;

        Vector3 attackPoint = transform.position + (Vector3)direction * 1f;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 previewPoint = transform.position + transform.right * 1f;
        Gizmos.DrawWireSphere(previewPoint, attackRange);
    }
}