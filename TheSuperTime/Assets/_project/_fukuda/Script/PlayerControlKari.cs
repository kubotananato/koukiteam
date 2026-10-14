using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlKari : MonoBehaviour
{
    // PlayerStatusを参照
    public PlayerStatus playerStatus;
    public float PmoveSpeed = 5f; // 歩く速度

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;
    Input input;

    void Start()
    {
        input = GetComponent<Input>();
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
        if (input.inputs["left"])
        {
            horizontal = -1f;
        }

        if (input.inputs["right"])
        {
            horizontal = 1f;
        }

        if (input.inputs["back"])
        {
            vertical = -1f;
        }

        if (input.inputs["forward"])
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
        transform.position += move * PmoveSpeed * input.gameDeltaTime;

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