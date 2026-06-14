using UnityEngine;

public class WorldChest : MonoBehaviour
{
    [Header("Налаштування Типу Сундука")]
    public bool isRoyal = false;    

    [Header("Спрайти для візуалу")]
    public Sprite normalChestSprite;
    public Sprite royalChestSprite;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (isRoyal && royalChestSprite != null)
            {
                sr.sprite = royalChestSprite;
            }
            else if (!isRoyal && normalChestSprite != null)
            {
                sr.sprite = normalChestSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            InventorySystem inventory = FindFirstObjectByType<InventorySystem>();

            if (stats != null && inventory != null)
            {
                ItemData droppedItem = LootManager.Instance.RollLoot(stats.luck, isRoyal);
                inventory.AddItem(droppedItem);

                string chestType = isRoyal ? "👑 КОРОЛІВСЬКОГО" : "📦 БЕЗКОШТОВНОГО";
                Debug.Log($"Гравець підібрав {chestType} сундук! Випало: {droppedItem.GetColoredName()}");

                Destroy(gameObject);
            }
        }
    }
}