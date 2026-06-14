using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && itemData != null)
        {
            sr.sprite = itemData.itemIcon;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "PlayerSprite")
        {
            bool pickedUp = InventorySystem.Instance.AddItem(itemData);

            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}