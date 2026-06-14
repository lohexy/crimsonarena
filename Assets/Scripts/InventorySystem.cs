using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [Header("Сумка (Інвентар)")]
    public List<ItemData> backpackItems = new List<ItemData>();
    public int maxBackpackSlots = 4;
    public Image[] backpackUISlots;

    [Header("Слоти Спорядження (Надіте на героя)")]
    public Image weaponSlotUI;
    public Image armorSlotUI;
    public Image helmetSlotUI;       
    public Image bootsSlotUI;        
    public Image accessorySlotUI;
    public Image leg_armorSlotUI;
    public Image shieldSlotUI;
    public Image ringSlotUI;
    public ItemData equippedWeapon;
    
    private ItemData equippedArmor;
    private ItemData equippedHelmet;
    private ItemData equippedBoots;
    private ItemData equippedAccessory;
    private ItemData equippedLeg_Armor;
    private ItemData equippedShield;
    private ItemData equippedRing;

    private PlayerStats playerStats;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        UpdateInventoryUI();
    }

    public bool AddItem(ItemData newItem)
    {
        if (backpackItems.Count >= maxBackpackSlots)
        {
            Debug.Log("Сумка повна!");
            return false;
        }

        backpackItems.Add(newItem);
        UpdateInventoryUI();
        return true;
    }

    public void ClickBackpackSlot(int slotIndex)
{
    if (slotIndex >= backpackItems.Count) return;

    ItemData itemToEquip = backpackItems[slotIndex];

    if (itemToEquip.itemType == ItemData.ItemType.Weapon)
    {
        if (equippedWeapon != null) UnequipCurrentItem(equippedWeapon);
        equippedWeapon = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Armor)
    {
        if (equippedArmor != null) UnequipCurrentItem(equippedArmor);
        equippedArmor = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Helmet)
    {
        if (equippedHelmet != null) UnequipCurrentItem(equippedHelmet);
        equippedHelmet = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Boots)
    {
        if (equippedBoots != null) UnequipCurrentItem(equippedBoots);
        equippedBoots = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Accessory)
    {
        if (equippedAccessory != null) UnequipCurrentItem(equippedAccessory);
        equippedAccessory = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.LegArmor)
    {
        if (equippedLeg_Armor != null) UnequipCurrentItem(equippedLeg_Armor);
        equippedLeg_Armor = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Shield)
    {
        if (equippedShield != null) UnequipCurrentItem(equippedShield);
        equippedShield = itemToEquip;
    }
    else if (itemToEquip.itemType == ItemData.ItemType.Ring)
    {
        if (equippedRing != null) UnequipCurrentItem(equippedRing);
        equippedRing = itemToEquip;
    }

    if (playerStats != null) playerStats.EquipItem(itemToEquip);

    backpackItems.RemoveAt(slotIndex);
    UpdateInventoryUI();
}

public void ClickEquippedSlot(string slotType)
{
    ItemData itemToUnequip = null;

    if (slotType == "Weapon") itemToUnequip = equippedWeapon;
    else if (slotType == "Armor") itemToUnequip = equippedArmor;
    else if (slotType == "Helmet") itemToUnequip = equippedHelmet;
    else if (slotType == "Boots") itemToUnequip = equippedBoots;
    else if (slotType == "Accessory") itemToUnequip = equippedAccessory;
    else if (slotType == "LegArmor") itemToUnequip = equippedLeg_Armor;
    else if (slotType == "Shield") itemToUnequip = equippedShield;
    else if (slotType == "Ring") itemToUnequip = equippedRing;

    if (itemToUnequip != null)
    {
        backpackItems.Add(itemToUnequip);

        if (playerStats != null) playerStats.UnequipItem(itemToUnequip);

        if (slotType == "Weapon") equippedWeapon = null;
        else if (slotType == "Armor") equippedArmor = null;
        else if (slotType == "Helmet") equippedHelmet = null;
        else if (slotType == "Boots") equippedBoots = null;
        else if (slotType == "Accessory") equippedAccessory = null;
        else if (slotType == "LegArmor") equippedLeg_Armor = null;
        else if (slotType == "Shield") equippedShield = null;
        else if (slotType == "Ring") equippedRing = null;

        UpdateInventoryUI();
    }
}

    void UnequipCurrentItem(ItemData oldItem)
    {
        if (playerStats != null) playerStats.UnequipItem(oldItem);
        backpackItems.Add(oldItem);
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < backpackUISlots.Length; i++)
        {
            if (i < backpackItems.Count)
            {
                backpackUISlots[i].sprite = backpackItems[i].itemIcon;
                backpackUISlots[i].color = Color.white;
            }
            else
            {
                backpackUISlots[i].sprite = null;
                backpackUISlots[i].color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            }
        }

        UpdateSingleEquipSlot(weaponSlotUI, equippedWeapon);
        UpdateSingleEquipSlot(armorSlotUI, equippedArmor);
        UpdateSingleEquipSlot(accessorySlotUI, equippedAccessory);
        UpdateSingleEquipSlot(helmetSlotUI, equippedHelmet);
        UpdateSingleEquipSlot(bootsSlotUI, equippedBoots);
        UpdateSingleEquipSlot(leg_armorSlotUI, equippedLeg_Armor);
        UpdateSingleEquipSlot(shieldSlotUI, equippedShield);
        UpdateSingleEquipSlot(ringSlotUI, equippedRing);
    }

    private void UpdateSingleEquipSlot(Image slotUI, ItemData equippedItem)
{
    if (slotUI == null) return; 

    if (equippedItem != null)
    {
        slotUI.sprite = equippedItem.itemIcon;
        slotUI.color = Color.white;
    }
    else
    {
        slotUI.sprite = null;
        slotUI.color = new Color(0.2f, 0.2f, 0.2f, 0.5f); 
    }
}
}