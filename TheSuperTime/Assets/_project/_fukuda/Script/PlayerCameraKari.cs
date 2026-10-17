using UnityEngine;

public class PlaeyerCameraKari : MonoBehaviour
{
    public float mouseSens = 2f;

    float xRotation;
    float initialXRotation;
    int lastReplayVersion;

    public GameObject HandReticle;
    public GameObject WeaponReticle;

    Input input;

    void Start()
    {
        input = transform.parent.GetComponent<Input>();

        // 初期の上下角度を保存
        initialXRotation = Mathf.DeltaAngle(
            0f,
            transform.localEulerAngles.x
        );

        xRotation = initialXRotation;
        lastReplayVersion = input.replayVersion;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 再生開始時・ループ時に上下角度を戻す
        if (lastReplayVersion != input.replayVersion)
        {
            xRotation = initialXRotation;
            lastReplayVersion = input.replayVersion;
        }

        Vector2 mouseDelta = input.mouseDelta;

        float mouseX = mouseDelta.x * mouseSens;
        float mouseY = mouseDelta.y * mouseSens;

        // 上下
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        // 左右
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}