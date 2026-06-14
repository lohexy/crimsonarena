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
            lastMousePosition = mousePosition;
            Attack();
        }
    }

    void Attack()
    {
        PlayerStats stats = GetComponentInParent<PlayerStats>();
        float currentRange = attackRange;
        int finalDamage = damage;
        bool isCrit = false;

        if (stats != null)
        {
            currentRange = stats.attackRangeMelee;
            finalDamage = stats.GetCalculatedDamage(true, out isCrit);
            
            attackRate = 0.6f / stats.attackSpeedMultiplier; 
        }

        nextAttackTime = Time.time + attackRate;

        Vector3 centerPoint = (transform.parent != null) ? transform.parent.position : transform.position;
        Vector2 direction = (lastMousePosition - (Vector2)centerPoint).normalized;
        
        Vector3 attackPoint = centerPoint + (Vector3)direction * 1f;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, currentRange, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (isCrit) Debug.Log("<color=yellow>💥 КРИТ МЕЧЕМ!</color>");
                enemy.TakeDamage(finalDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 centerPoint = (transform.parent != null) ? transform.parent.position : transform.position;
        Gizmos.DrawWireSphere(centerPoint + transform.right * 1f, attackRange);
    }
}