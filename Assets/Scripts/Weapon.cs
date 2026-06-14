using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName = "Посох";
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    
    private float nextFireTime = 0f;

    public void Fire(Vector2 mousePosition)
{
    if (Time.time >= nextFireTime)
    {
        nextFireTime = Time.time + fireRate;
        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        PlayerStats stats = GetComponentInParent<PlayerStats>();
if (stats != null && stats.currentClass != null && stats.currentClass.className == "Маг")
{
    Transform closestEnemy = FindClosestEnemy();
    if (closestEnemy != null)
    {
        direction = ((Vector2)closestEnemy.position - (Vector2)transform.position).normalized;
    }
}
        
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null) projectileScript.Setup(direction);
    }
}

private Transform FindClosestEnemy()
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    Transform closest = null;
    float distance = Mathf.Infinity;
    Vector3 position = transform.position;

    foreach (GameObject enemy in enemies)
    {
        Vector3 diff = enemy.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
            closest = enemy.transform;
            distance = curDistance;
        }
    }
    return closest;
}
}