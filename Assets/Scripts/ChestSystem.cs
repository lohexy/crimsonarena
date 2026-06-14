using UnityEngine;

public class ChestSystem : MonoBehaviour
{
    public PlayerStats playerStats;
    public InventorySystem inventory;
    public void BuyNormalChest()
    {
        if (playerStats.coins >= 10)
        {
            playerStats.coins -= 10;
            ItemData droppedItem = LootManager.Instance.RollLoot(playerStats.luck, false);
            
            inventory.AddItem(droppedItem); 
            playerStats.UpdateUI();
            Debug.Log($"📦 Відкрив Бичівський сундук! Випало: {droppedItem.GetColoredName()}");
        }
        else
        {
            Debug.Log("❌ Недостатньо монет для Бичівського сундуку!");
        }
    }

    public void BuyRoyalChest()
    {
        if (playerStats.coins >= 50)
        {
            playerStats.coins -= 50;
            ItemData droppedItem = LootManager.Instance.RollLoot(playerStats.luck, true);
            
            inventory.AddItem(droppedItem);
            playerStats.UpdateUI();
            Debug.Log($"Випало: {droppedItem.GetColoredName()}");
        }
        else
        {
            Debug.Log("❌ Недостатньо монет на Королівський!");
        }
    }
}