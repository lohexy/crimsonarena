using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "FantasyGame/Inventory Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public enum WeaponType { None, Melee, Ranged }

    public enum ItemType { 
        Weapon, 
        Armor, 
        Helmet, 
        Boots, 
        Accessory,
        LegArmor,
        Shield,
        Ring
    }
    
    public WeaponType weaponType;
    public ItemType itemType;

    [Header("Качество предмета")]
    [Range(1, 4)] public int itemTier = 1;
    public int cost = 10;

    [Header(" Бонуси до Атаки")]
    public int damageBonus = 0;
    public float attackRangeBonus = 0f;
    public float attackSpeedMultiplierBonus = 0f; 
    public float critChanceBonus = 0f;            
    public float critDamageMultiplierBonus = 0f;  

    [Header(" Бонуси до Захисту")]
    public int healthBonus = 0;
    public float hpRegenBonus = 0f;
    public float dodgeChanceBonus = 0f;           
    public float blockChanceBonus = 0f;           
    public int armorBonus = 0; 

    [Header(" Пересування та Утиліти")]
    public float moveSpeedBonus = 0f;
    public float goldMultiplierBonus = 0f;
    public float xpMultiplierBonus = 0f;
    public float luckBonus = 0f;

    [Header(" Налаштування Ближнього бою (Melee)")]
    public bool useCustomSlashColor = false;
    public Gradient slashTrailColor;
    [Tooltip("Максимальний множник шкоди при ідеальному замаху (наприклад, 2.0 = +100% урону)")]
    public float maxSwingDamageMultiplier = 2.0f;

    [Header(" Налаштування Дальнього бою (Ranged)")]
    [Tooltip("Унікальний снаряд цієї зброї (стріла, фаєрбол, куля тощо)")]
    public GameObject projectilePrefab;
    [Tooltip("Скільки секунд потрібно утримувати кнопку для максимального заряду")]
    public float maxChargeTime = 1.0f;
    [Tooltip("Множник шкоди при максимальному заряді (наприклад, 1.5)")]
    public float maxChargeDamageMultiplier = 1.5f;
    [Tooltip("Множник розміру снаряду при максимальному заряді (наприклад, 1.5)")]
    public float maxChargeSizeMultiplier = 1.5f;

    public string GetColoredName()
    {
        switch (itemTier)
        {
            case 1: return $"<color=white>{itemName}</color>";
            case 2: return $"<color=#00FF00>{itemName}</color>";
            case 3: return $"<color=#00C0FF>{itemName}</color>";
            case 4: return $"<color=#FF9900>{itemName}</color>";
            default: return itemName;
        }
    }
}