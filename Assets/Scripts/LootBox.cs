using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    [Header("Всі можливі шмотки в грі")]
    public List<ItemData> allItems = new List<ItemData>();

    void Awake()
    {
        Instance = this;
    }

    public ItemData RollLoot(float playerLuck, bool isRoyalChest)
    {
        float chanceT1 = 60f;
        float chanceT2 = 25f;
        float chanceT3 = 10f;
        float chanceT4 = 5f;

        chanceT4 *= (playerLuck * playerLuck); 
        chanceT3 *= playerLuck;
        
        if (isRoyalChest)
        {
            chanceT1 = 0f;
        }

        float totalWeight = chanceT1 + chanceT2 + chanceT3 + chanceT4;
        float randomRoll = Random.Range(0f, totalWeight);

        int selectedTier = 1;

        if (randomRoll <= chanceT4) selectedTier = 4;
        else if (randomRoll <= chanceT4 + chanceT3) selectedTier = 3;
        else if (randomRoll <= chanceT4 + chanceT3 + chanceT2) selectedTier = 2;
        else selectedTier = 1;

        List<ItemData> validItems = allItems.FindAll(item => item.itemTier == selectedTier);

        if (validItems.Count == 0) return allItems[Random.Range(0, allItems.Count)];

        return validItems[Random.Range(0, validItems.Count)];
    }
}