using UnityEngine;
using UnityEngine.InputSystem;

public class PlaeyerCameraKari : MonoBehaviour
{
    public float mouseSens = 2f;
    private float xRotation = 0f;

    public GameObject HandReticle;
    public GameObject WeaponReticle;
    Input input;
  

    void Start()
    {
        GameObject p = transform.parent.gameObject;
        input = p.GetComponent<Input>();
        // マウスカーソルを画面に中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        // マウスカーソルを画面上に表示しない
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = input.mouseDelta;

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