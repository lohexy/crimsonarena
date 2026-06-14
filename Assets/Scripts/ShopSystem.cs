using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public PlayerStats playerStats;
    public InventorySystem inventory;

    [Header("4 Слоти Товарів")]
    public List<ItemData> shopSlots = new List<ItemData>(new ItemData[4]);

    [Header("UI Елементи Слотов (Має бути 4 штуки)")]
    public List<Image> slotIcons;        
    public List<TextMeshProUGUI> priceTexts; 

    [Header("Таймер")]
    public TextMeshProUGUI timerText;
    private float refreshTimer = 180f;

    void Start()
    {
        RefreshShopGoods();
    }

    void Update()
    {
        refreshTimer -= Time.deltaTime;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(refreshTimer / 60f);
            int seconds = Mathf.FloorToInt(refreshTimer % 60f);
            timerText.text = $"Оновлення: {minutes:00}:{seconds:00}";
        }

        if (refreshTimer <= 0)
        {
            RefreshShopGoods();
        }
    }

    public void RefreshShopGoods()
    {
        refreshTimer = 180f;

        for (int i = 0; i < 4; i++)
        {
            shopSlots[i] = LootManager.Instance.RollLoot(playerStats.luck, false);
        }

        UpdateShopUI();
    }

    void UpdateShopUI()
    {
        for (int i = 0; i < 4; i++)
        {
            if (shopSlots[i] != null)
            {
                slotIcons[i].sprite = shopSlots[i].itemIcon;
                slotIcons[i].enabled = true; 

                if (priceTexts != null && i < priceTexts.Count && priceTexts[i] != null)
                {
                    priceTexts[i].text = $"{shopSlots[i].cost} монет";
                }
            }
            else
            {
                slotIcons[i].enabled = false; 
                if (priceTexts != null && i < priceTexts.Count && priceTexts[i] != null)
                {
                    priceTexts[i].text = "Куплено";
                }
            }
        }
    }

    public void BuyItemFromShop(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= shopSlots.Count || shopSlots[slotIndex] == null) return;

        ItemData itemToBuy = shopSlots[slotIndex];

        if (playerStats.coins >= itemToBuy.cost)
        {
            playerStats.coins -= itemToBuy.cost;
            inventory.AddItem(itemToBuy);

            shopSlots[slotIndex] = null; 
            playerStats.UpdateUI();
            UpdateShopUI();
        }
        else
        {
            Debug.Log("Не вистачає монет!");
        }
    }

    public void BuyNormalChest()
    {
        if (playerStats.coins >= 10)
        {
            playerStats.coins -= 10;
            
            ItemData droppedItem = LootManager.Instance.RollLoot(playerStats.luck, false);
            inventory.AddItem(droppedItem);
            
            playerStats.UpdateUI();
            Debug.Log($"📦 Куплено Звичайний сундук! Випало: {droppedItem.GetColoredName()}");
        }
        else
        {
            Debug.Log("Недостатньо монет на сундук!");
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
            Debug.Log($"👑 Куплено Королівський сундук! Випало: {droppedItem.GetColoredName()}");
        }
        else
        {
            Debug.Log("Недостатньо монет на Королівський сундук!");
        }
    }
}