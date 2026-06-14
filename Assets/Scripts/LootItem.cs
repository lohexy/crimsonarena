using UnityEngine;

public class LootItem : MonoBehaviour
{
    public enum ItemType { Experience, Bronze, Silver, Gold }
    
    [Header("Тип предмету та його цінність")]
    public ItemType type; 
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "PlayerSprite")
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();

            if (stats != null)
            {
                switch (type)
                {
                    case ItemType.Experience:
                        stats.AddXp(value);
                        break;
                        
                    case ItemType.Bronze:
                    case ItemType.Silver:
                    case ItemType.Gold:
                        stats.AddCoins(value);
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}