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

        RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void RotateTowardsMouse()
    {
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Екіпіровано нову зброю: {newWeapon.weaponName}");
    }
}