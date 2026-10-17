using UnityEngine;

public class PlayerControlKari : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public float moveSpeed = 5f;

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;

    Input input;

    void Start()
    {
        input = GetComponent<Input>();
    }

    void Update()
    {
        if (input.IsReplay)
        {
            Time.timeScale = 1f;
        }

        if (playerStatus.isDead)
        {
            return;
        }

        Vector3 move =
            transform.forward * input.moveAxis.y +
            transform.right * input.moveAxis.x;

        transform.position +=
            move * moveSpeed * input.gameDeltaTime;

        // リプレイ中はスロー処理を実行しない
        if (input.IsReplay)
        {
            return;
        }

        if (move.sqrMagnitude > 0f || NormalTimer > 0f)
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

    // 銃を撃ったときに呼ぶ
    public void OnShoot()
    {
        if (input.IsReplay) return;

        NormalTimer = NormalDuration;
        Time.timeScale = 1f;
    }
}