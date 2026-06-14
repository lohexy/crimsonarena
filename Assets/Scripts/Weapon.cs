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
            
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Setup(direction);
            }
        }
    }
}