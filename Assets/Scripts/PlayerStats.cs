using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Характеристики")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Система Рівнів та Експи")]
    public int currentLevel = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    [Header("Ресурси")]
    public int coins = 0;

    [Header("Елементи HUD (UI)")]
    public Slider healthSlider;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    [Header("Бонуси від спорядження")]
    public int bonusDamage = 0;
    public int bonusArmor = 0;

    void Start()
    {
        if (MainMenu.SelectedClass != null)
        {
            maxHealth = MainMenu.SelectedClass.startHealth;
        }

        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (healthSlider != null) healthSlider.value = currentHealth;

        Debug.Log("Гравець отримав урон! Поточне HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Гравець ЗАГИНУВ!");
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    public void AddXp(int amount)
    {
        currentXp += amount;

        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;
        
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f); 

        currentHealth = maxHealth;

        Debug.Log($"<color=green>LEVEL UP! Поточний рівень: {currentLevel}</color>");
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXp;
        }

        if (levelText != null) levelText.text = "LVL: " + currentLevel;
        if (coinText != null) coinText.text = "GOLD: " + coins;
    }

    public int GetTotalDamage()
    {
        int baseDamage = 20;
        return baseDamage + bonusDamage;
    }

    public void EquipItem(ItemData item)
    {
        if (item == null) return;

        maxHealth += item.healthBonus;
        bonusDamage += item.damageBonus;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        Debug.Log($"Одягнено: {item.itemName}. Бонус шкоди: +{item.damageBonus}, Бонус ХП: +{item.healthBonus}");
    }

    public void UnequipItem(ItemData item)
    {
        if (item == null) return;

        maxHealth -= item.healthBonus;
        bonusDamage -= item.damageBonus;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        Debug.Log($"Знято: {item.itemName}. Бонуси скасовано.");
    }
}