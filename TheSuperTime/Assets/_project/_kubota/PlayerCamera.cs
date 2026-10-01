using UnityEngine;
using UnityEngine.InputSystem;

public class PlaeyerCamera : MonoBehaviour
{
    public float mouseSens = 2f;
    private float xRotation = 0f;

    public GameObject WeaponReticle;
    public GameObject HandReticle;

    void Start()
    {
        // マウスカーソルを画面に中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        // マウスカーソルを画面上に表示しない
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSens;
        float mouseY = mouseDelta.y * mouseSens;

        // 上下
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 左右
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}