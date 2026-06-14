using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Weapon currentWeapon;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
        rb.freezeRotation = true;

        if (currentWeapon == null)
        {
            currentWeapon = GetComponentInChildren<Weapon>();
        }
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Camera.main != null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // 🔥 ВИКЛИКАЄМО ОБЕРТАННЯ ЗА МИШКОЮ
        RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // 🔥 НОВИЙ МЕТОД ДЛЯ ОБЕРТАННЯ ЛИЦЯРЯ
    void RotateTowardsMouse()
    {
        // Рахуємо напрямок від гравця до курсора миші
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        
        // Вираховуємо кут в градусах
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        
        // Оскільки твоя зброя та спрайт знаходяться на одному об'єкті й мають дивитися в один бік:
        // Якщо спрайт лицаря за замовчуванням дивиться ВПРАВО — залишай просто angle.
        // Якщо лицар спочатку дивиться ВГОРУ — допиши: angle - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Екіпіровано нову зброю: {newWeapon.weaponName}");
    }
}