using UnityEngine;
using System.Collections;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Зв'язок з інвентарем та статами")]
    public InventorySystem inventory;
    private PlayerStats playerStats;

    [Header("Об'єкти зброї на сцені")]
    public Transform weaponPivot;
    public SpriteRenderer weaponVisual;

    [Header("Налаштування дефолтного дальнього бою")]
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    [Tooltip("Снаряд, який вилітає, якщо у ItemData не призначено власного префабу")]
    public GameObject defaultProjectilePrefab; 

    [Header("Налаштування ближнього бою (Меч)")]
    public LayerMask enemyLayer;
    public float baseMeleeAttackRate = 0.6f;

    [Header(" Налаштування механіки замаху")]
    public float swingDuration = 0.15f; 
    public float minSwingAngle = 45f;
    public float maxSwingAngle = 180f;

    [Header(" Візуальний ефект змаху меча")]
    public TrailRenderer swordTrail; 

    private float chargeTimer = 0f;
    private bool isCharging = false;

    private Coroutine attackCoroutine;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        UpdateTrailSettings();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        UpdateWeaponVisualAndRotation(mousePosition);

        HandleInput(mousePosition);
    }

    void UpdateWeaponVisualAndRotation(Vector2 mousePosition)
    {
        if (weaponVisual == null || weaponPivot == null) return;

        if (inventory != null && inventory.equippedWeapon != null)
        {
            if (weaponVisual.sprite != inventory.equippedWeapon.itemIcon)
            {
                weaponVisual.sprite = inventory.equippedWeapon.itemIcon;
                UpdateTrailSettings();
            }

            weaponVisual.enabled = true;

            Vector2 direction = (mousePosition - (Vector2)weaponPivot.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            weaponVisual.sprite = null;
            weaponVisual.enabled = false;
            if (swordTrail != null) swordTrail.emitting = false;
        }
    }

    private void UpdateTrailSettings()
    {
        if (swordTrail == null) return;

        if (inventory == null || inventory.equippedWeapon == null || inventory.equippedWeapon.weaponType != ItemData.WeaponType.Melee)
        {
            swordTrail.emitting = false;
            return;
        }

        if (inventory.equippedWeapon.useCustomSlashColor)
        {
            swordTrail.colorGradient = inventory.equippedWeapon.slashTrailColor;
        }
        else
        {
            Gradient defaultGradient = new Gradient();
            defaultGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            swordTrail.colorGradient = defaultGradient;
        }
    }

    void HandleInput(Vector2 mousePosition)
    {
        if (inventory == null || inventory.equippedWeapon == null)
        {
            isCharging = false;
            return;
        }

        if (inventory.equippedWeapon.weaponType == ItemData.WeaponType.Melee)
        {
            isCharging = false; // На всяк випадок скидаємо заряд лука

            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                float currentAttackRate = baseMeleeAttackRate;
                if (playerStats != null) currentAttackRate = baseMeleeAttackRate / playerStats.attackSpeedMultiplier;

                nextFireTime = Time.time + currentAttackRate;

                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(MeasureMeleeSwingRoutine());
            }
        }
        else if (inventory.equippedWeapon.weaponType == ItemData.WeaponType.Ranged)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                isCharging = true;
                chargeTimer = 0f;
            }

            if (Input.GetButton("Fire1") && isCharging)
            {
                chargeTimer += Time.deltaTime;
            }

            if (Input.GetButtonUp("Fire1") && isCharging)
            {
                isCharging = false;

                float currentFireRate = fireRate;
                if (playerStats != null) currentFireRate = fireRate / playerStats.attackSpeedMultiplier;
                nextFireTime = Time.time + currentFireRate;

                FireRangedProjectile(mousePosition, chargeTimer);
            }
        }
    }

    void FireRangedProjectile(Vector2 mousePosition, float heldTime)
    {
        ItemData weapon = inventory.equippedWeapon;
        
        GameObject projectileToSpawn = weapon.projectilePrefab != null ? weapon.projectilePrefab : defaultProjectilePrefab;
        if (projectileToSpawn == null) return;

        float chargePct = Mathf.Clamp01(heldTime / weapon.maxChargeTime);

        GameObject projectile = Instantiate(projectileToSpawn, weaponVisual.transform.position, Quaternion.identity);
        Vector2 direction = (mousePosition - (Vector2)weaponVisual.transform.position).normalized;

        if (playerStats != null && playerStats.currentClass != null && playerStats.currentClass.className == "Маг")
        {
            Transform closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                direction = ((Vector2)closestEnemy.position - (Vector2)weaponVisual.transform.position).normalized;
            }
        }

        float finalSizeMultiplier = Mathf.Lerp(1f, weapon.maxChargeSizeMultiplier, chargePct);
        projectile.transform.localScale *= finalSizeMultiplier;

        int baseDamage = 5;
        bool isCrit = false;
        if (playerStats != null) baseDamage = playerStats.GetCalculatedDamage(false, out isCrit);

        float finalDamageMultiplier = Mathf.Lerp(1f, weapon.maxChargeDamageMultiplier, chargePct);
        int finalCalculatedDamage = Mathf.RoundToInt(baseDamage * finalDamageMultiplier);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Setup(direction);
        }

        if (chargePct > 0.1f)
        {
            Debug.Log($"<color=#FFCC00> Постріл! Заряд: {chargePct * 100:F0}%. Розмір снаряду: x{finalSizeMultiplier:F2}. Шкода: {finalCalculatedDamage}</color>");
        }
    }

    private IEnumerator MeasureMeleeSwingRoutine()
    {
        ItemData weapon = inventory.equippedWeapon;
        if (swordTrail != null) swordTrail.emitting = true;

        float startAngle = weaponPivot.eulerAngles.z;
        float totalTrackedAngle = 0f;
        float lastAngle = startAngle;
        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            yield return null;
            elapsed += Time.deltaTime;

            float currentAngle = weaponPivot.eulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(lastAngle, currentAngle);
            totalTrackedAngle += Mathf.Abs(deltaAngle); 
            lastAngle = currentAngle;
        }

        if (swordTrail != null) swordTrail.emitting = false;

        float currentAngleCleared = Mathf.Clamp(totalTrackedAngle, 0f, maxSwingAngle);
        float swingBonusMultiplier = 1f; 

        if (currentAngleCleared >= minSwingAngle)
        {
            float t = (currentAngleCleared - minSwingAngle) / (maxSwingAngle - minSwingAngle);
            swingBonusMultiplier = Mathf.Lerp(1f, weapon.maxSwingDamageMultiplier, t);
        }

        float currentRange = 1.5f;
        int baseDamage = 5;
        bool isCrit = false;

        if (playerStats != null)
        {
            currentRange = playerStats.attackRangeMelee;
            baseDamage = playerStats.GetCalculatedDamage(true, out isCrit);
        }

        int finalCalculatedDamage = Mathf.RoundToInt(baseDamage * swingBonusMultiplier);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)weaponPivot.position).normalized;
        Vector3 attackPoint = weaponPivot.position + (Vector3)direction * 1f;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, currentRange, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (isCrit) Debug.Log("<color=yellow>💥 КРИТ МЕЧЕМ!</color>");
                
                if (swingBonusMultiplier > 1.05f)
                {
                    Debug.Log($"<color=#00FFCC> Меч {weapon.itemName} | Розмах: {Mathf.RoundToInt(totalTrackedAngle)}° | Множник: x{swingBonusMultiplier:F2} | Нанесено: {finalCalculatedDamage}</color>");
                }

                enemy.TakeDamage(finalCalculatedDamage);
            }
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