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

    [Header("Бонуси")]
    public int healthBonus = 0;
    public int damageBonus = 0;
}