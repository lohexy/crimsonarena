using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private MeleeWeapon meleeWeaponScript;
    private Weapon rangedWeaponScript;

    [Header("Зв'язок з інвентарем")]
    public InventorySystem inventory;

    [Header("Візуальное відображення зброї")]
    public SpriteRenderer weaponSpriteRenderer;

    void Start()
    {
        meleeWeaponScript = GetComponent<MeleeWeapon>();
        rangedWeaponScript = GetComponent<Weapon>();

        if (meleeWeaponScript == null) Debug.LogError("MeleeWeapon не знайдено на Гравці!");
        if (rangedWeaponScript == null) Debug.LogError("Weapon (дальній бій) не знайдено на Гравці!");
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        UpdateWeaponVisual(mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            if (inventory != null && inventory.equippedWeapon != null)
            {
                if (inventory.equippedWeapon.weaponType == ItemData.WeaponType.Ranged)
                {
                    if (rangedWeaponScript != null) 
                    {
                        rangedWeaponScript.Fire(mousePosition);
                    }
                }
                else if (inventory.equippedWeapon.weaponType == ItemData.WeaponType.Melee)
                {
                    if (meleeWeaponScript != null) 
                    {
                        meleeWeaponScript.Fire(mousePosition);
                    }
                }
            }
        }
    }

    void UpdateWeaponVisual(Vector2 mousePosition)
    {
        if (weaponSpriteRenderer == null) return;

        if (inventory != null && inventory.equippedWeapon != null)
        {
            weaponSpriteRenderer.sprite = inventory.equippedWeapon.itemIcon;
            weaponSpriteRenderer.enabled = true;

            Vector2 direction = (mousePosition - (Vector2)weaponSpriteRenderer.transform.position).normalized;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            weaponSpriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (mousePosition.x < transform.position.x)
            {
                weaponSpriteRenderer.flipY = true;
            }
            else
            {
                weaponSpriteRenderer.flipY = false;
            }
        }
        else
        {
            weaponSpriteRenderer.sprite = null;
            weaponSpriteRenderer.enabled = false;
        }
    }
}