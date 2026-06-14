using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 🔥 ДОДАНО: Потрібно для перезапуску сцени

public class PlayerStats : MonoBehaviour
{
    [Header("Конфігурація Класу (ScriptableObject)")]
    public CharacterClass currentClass; 

    [Header("Очки для прокачки")]
    public int statPoints = 0; 

    [Header("Поточні характеристики")]
    public int maxHealth;
    public int currentHealth;
    public float hpRegenPerSecond; 
    private float regenTimer = 0f;

    [Header("Система Рівнів та Досвіду")]
    public int currentLevel = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    [Header("Ресурси та Множники")]
    public int coins = 0;
    public float goldMultiplier;  
    public float xpMultiplier;    
    public float luck;            

    [Header("Параметри Бою")]
    public int baseDamage;
    public int bonusDamage = 0;          
    public float damageMultiplier; 
    public float attackRangeMelee;
    public float attackRangeRanged;
    public float attackSpeedMultiplier;

    [Header("Крити / Додж / Блок")]
    public float critChance;     
    public float critDamageMultiplier;
    public float dodgeChance;    
    public float blockChance;    
    public int bonusArmor = 0;           

    [Header("Пересування")]
    public float moveSpeed;

    [Header("Елементи HUD (UI)")]
    public Slider healthSlider;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI statPointsText; 

    [Header("Вікна станів гри")]
    public GameObject gameOverPanel; // 🔥 ДОДАНО: Перетягни сюди UI-панель поразки з Canvas

    private int attackCounter = 0;
    private bool isInvulnerable = false;
    private float invulnTimer = 0f;

    void Start()
    {
        // Переконуємось, що час іде після рестарту або виходу з паузи
        Time.timeScale = 1f; 

        // 🔥 ОСЬ ЦЕЙ ЗВ'ЯЗОК З ТВОЇМ МЕНЮ:
        // Перевіряємо, чи у скрипті MainMenu була заповнена статична змінна
        if (MainMenu.SelectedClass != null)
        {
            currentClass = MainMenu.SelectedClass; // Беремо клас, який ти обрав кнопкою!
        }

        ApplyClassSettings(); // Застосовуємо ХП, демедж та СПРАЙТ нового класу
        currentHealth = maxHealth;
        SyncMoveSpeed();
        
        if (gameOverPanel != null) 
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    void ApplyClassSettings()
    {
        if (currentClass == null) return;

        maxHealth = currentClass.startMaxHealth;
        hpRegenPerSecond = currentClass.startHpRegen;
        moveSpeed = currentClass.startMoveSpeed;
        baseDamage = currentClass.startBaseDamage;
        attackRangeMelee = currentClass.attackRangeMelee;
        attackRangeRanged = currentClass.attackRangeRanged;
        attackSpeedMultiplier = currentClass.attackSpeedMultiplier;
        damageMultiplier = currentClass.damageMultiplier;
        critChance = currentClass.startCritChance;
        critDamageMultiplier = currentClass.critDamageMultiplier;
        dodgeChance = currentClass.startDodgeChance;
        blockChance = currentClass.startBlockChance;
        goldMultiplier = currentClass.goldMultiplier;
        xpMultiplier = currentClass.xpMultiplier;
        luck = currentClass.luck;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && currentClass.characterSprite != null)
        {
            sr.sprite = currentClass.characterSprite; 
        }
    }

    void Update()
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= 1f)
            {
                currentHealth += Mathf.RoundToInt(hpRegenPerSecond);
                if (currentHealth > maxHealth) currentHealth = maxHealth;
                regenTimer = 0f;
                UpdateUI();
            }
        }

        if (isInvulnerable)
        {
            invulnTimer -= Time.deltaTime;
            if (invulnTimer <= 0f) isInvulnerable = false;
        }
    }

    public void UpgradeHealth()
    {
        if (statPoints > 0)
        {
            maxHealth += 10;
            currentHealth = maxHealth; 

            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeRegen()
    {
        if (statPoints > 0 && hpRegenPerSecond < currentClass.regenMax)
        {
            hpRegenPerSecond += currentClass.regenStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeDamage()
    {
        if (statPoints > 0 && baseDamage < currentClass.damageMax)
        {
            baseDamage += currentClass.damageStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeSpeed()
    {
        if (statPoints > 0 && moveSpeed < currentClass.speedMax)
        {
            moveSpeed += currentClass.speedStep;
            statPoints--;
            SyncMoveSpeed();
            UpdateUI();
        }
    }

    public void UpgradeCrit()
    {
        if (statPoints > 0 && critChance < currentClass.critMax)
        {
            critChance += currentClass.critStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeCritDamage()
    {
        if (statPoints > 0 && critDamageMultiplier < currentClass.critDmgMax)
        {
            critDamageMultiplier += currentClass.critDmgStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeDodge()
    {
        if (statPoints > 0 && dodgeChance < currentClass.dodgeMax)
        {
            dodgeChance += currentClass.dodgeStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeBlock()
    {
        if (statPoints > 0 && blockChance < currentClass.blockMax)
        {
            blockChance += currentClass.blockStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void UpgradeLuck()
    {
        if (statPoints > 0 && luck < currentClass.luckMax)
        {
            luck += currentClass.luckStep;
            statPoints--;
            UpdateUI();
        }
    }

    public void EquipItem(ItemData item)
    {
        if (item == null) return;

        maxHealth += item.healthBonus; 
        currentHealth += item.healthBonus;
        
        if (maxHealth < 1) maxHealth = 1;
        if (currentHealth < 1) currentHealth = 1;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        hpRegenPerSecond += item.hpRegenBonus;

        bonusDamage += item.damageBonus;
        attackRangeMelee += item.attackRangeBonus; 
        attackRangeRanged += item.attackRangeBonus; 
        attackSpeedMultiplier += item.attackSpeedMultiplierBonus;

        critChance += item.critChanceBonus;
        critDamageMultiplier += item.critDamageMultiplierBonus;
        dodgeChance += item.dodgeChanceBonus;
        blockChance += item.blockChanceBonus;
        bonusArmor += item.armorBonus;

        moveSpeed += item.moveSpeedBonus; 
        goldMultiplier += item.goldMultiplierBonus;
        xpMultiplier += item.xpMultiplierBonus;
        luck += item.luckBonus;

        SyncMoveSpeed();
        UpdateUI();
    }

    public void UnequipItem(ItemData item)
    {
        if (item == null) return;

        maxHealth -= item.healthBonus; 
        currentHealth -= item.healthBonus;
        
        if (maxHealth < 1) maxHealth = 1;
        if (currentHealth < 1) currentHealth = 1;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        hpRegenPerSecond -= item.hpRegenBonus;

        bonusDamage -= item.damageBonus;
        attackRangeMelee -= item.attackRangeBonus;
        attackRangeRanged -= item.attackRangeBonus;
        attackSpeedMultiplier -= item.attackSpeedMultiplierBonus;

        critChance -= item.critChanceBonus;
        critDamageMultiplier -= item.critDamageMultiplierBonus;
        dodgeChance -= item.dodgeChanceBonus;
        blockChance -= item.blockChanceBonus;
        bonusArmor -= item.armorBonus;

        moveSpeed -= item.moveSpeedBonus;
        goldMultiplier -= item.goldMultiplierBonus;
        xpMultiplier -= item.xpMultiplierBonus;
        luck -= item.luckBonus;

        if (critChance < 0f) critChance = 0f;
        if (dodgeChance < 0f) dodgeChance = 0f;
        if (blockChance < 0f) blockChance = 0f;

        SyncMoveSpeed();
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Якщо вже мертвий, шкоду не приймаємо
        if (isInvulnerable) return;
        if (Random.value <= dodgeChance) return;

        int finalDamage = damage - bonusArmor;
        if (Random.value <= blockChance) finalDamage = Mathf.RoundToInt(finalDamage * 0.5f); 

        if (finalDamage < 1) finalDamage = 1;
        currentHealth -= finalDamage;
        
        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            UpdateUI();
            Die(); // 🔥 ВИКЛИК СМЕРТІ
            return;
        }

        UpdateUI();

        if (currentClass != null && currentClass.className == "Воїн" && currentHealth > 0 && currentHealth < (maxHealth * 0.3f) && !isInvulnerable)
        {
            TriggerInvulnerability(3f);
        }
    }

    public void TriggerInvulnerability(float duration) { isInvulnerable = true; invulnTimer = duration; }

    public int GetCalculatedDamage(bool isMeleeAttack, out bool wasCrit)
    {
        wasCrit = false;
        float totalDmg = (baseDamage + bonusDamage) * damageMultiplier;

        if (currentClass != null && currentClass.className == "Лучник" && !isMeleeAttack)
        {
            attackCounter++;
            if (attackCounter >= 5) { attackCounter = 0; wasCrit = true; return Mathf.RoundToInt(totalDmg * critDamageMultiplier); }
        }

        if (Random.value <= critChance) { wasCrit = true; return Mathf.RoundToInt(totalDmg * critDamageMultiplier); }
        return Mathf.RoundToInt(totalDmg);
    }

    public void AddXp(int amount)
    {
        currentXp += Mathf.RoundToInt(amount * xpMultiplier);
        while (currentXp >= xpToNextLevel) { LevelUp(); }
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += Mathf.RoundToInt(amount * goldMultiplier);
        UpdateUI();
    }

    void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.25f); 

        statPoints += 3; 

        UpdateUI();
    }

    void SyncMoveSpeed()
    {
        PlayerController moveScript = GetComponent<PlayerController>();
        if (moveScript != null) moveScript.moveSpeed = moveSpeed; 
    }

    public void UpdateUI()
    {
        if (healthSlider != null) { healthSlider.maxValue = maxHealth; healthSlider.value = currentHealth; }
        if (xpSlider != null) { xpSlider.maxValue = xpToNextLevel; xpSlider.value = currentXp; }
        if (levelText != null) levelText.text = "Lvl: " + currentLevel;
        if (coinText != null) coinText.text = coins.ToString();
        if (statPointsText != null) statPointsText.text = "Очки статів: " + statPoints;
    }

    // 🔥 ДОДАНО: Метод смерті гравця
    void Die()
    {
        Debug.Log("Гравець загинув під час тестів!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Вмикаємо UI вікно
        }

        Time.timeScale = 0f; // Зупиняємо ігровий процес (анімації, спавн хвиль, фізику)
    }

    // 🔥 ДОДАНО: Метод для кнопки Restart на панелі Game Over
    public void RestartGame()
    {
        Time.timeScale = 1f; // Повертаємо час у норму
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапускаємо поточну сцену
    }

    // 🔥 ДОДАНО: Метод для кнопки виходу в меню
    public void GoToMenu()
    {
       Time.timeScale = 1f;
        // Тепер назва точно збігається з налаштуваннями твоїх Build Profiles!
        SceneManager.LoadScene("MainMenuScene"); // Заміни на точну назву сцени свого головного меню
    }
}