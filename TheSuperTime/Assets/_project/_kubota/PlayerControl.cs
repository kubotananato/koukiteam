using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // PlayerStatusを参照
    public PlayerStatus playerStatus;
    public float PmoveSpeed = 5f; // 歩く速度

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;

    void Start()
    {

    }

    void Update()
    {
        if (playerStatus.isDead)
        {
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        // WASD移動
        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1f;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1f;
        }

        // Playerの向いている方向を基準にする
        Vector3 move =
            transform.forward * vertical +
            transform.right * horizontal;

        // 斜め移動が速くならないようにする
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // 移動
        transform.position += move * PmoveSpeed * Time.deltaTime;

        // 移動している、または撃った直後なら通常速度。それ以外はスロー
        if (move.magnitude > 0f || NormalTimer > 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.05f;
        }

        if (NormalTimer > 0f)
        {
            NormalTimer -= Time.unscaledDeltaTime;
        }
    }

    // 銃を撃ったときに PlayerGun.cs から呼ばれる
    public void OnShoot()
    {
        NormalTimer = NormalDuration;
        Time.timeScale = 1f;
    }
}