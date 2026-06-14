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
//
   //     if (Input.GetButton("Fire1") && currentWeapon != null) 
   //     {
  //          currentWeapon.Fire(mousePos);
   //     }
}

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Екіпіровано нову зброю: {newWeapon.weaponName}");
    }
}