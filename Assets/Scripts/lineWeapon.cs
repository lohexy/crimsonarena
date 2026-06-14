using UnityEngine;

public class lineWeapon : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        transform.localScale = Vector3.one;
    }

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        if (mainCamera == null) return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 lookDirection = mousePosition - transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}