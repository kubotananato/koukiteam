
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public float PmoveSpeed = 5f;

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;

    private Rigidbody rb;
    Input input;
    Vector3 move;
    PlayerStatus status;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("PlayerにRigidbodyがありません！");
        }

        input = GetComponent<Input>();
        status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        if(input.IsReplay || status.isDead)
        {
            Time.timeScale = 1.0f;
        }
        if (playerStatus != null && playerStatus.isDead)
            return;
        if (Keyboard.current == null)
            return;

        /*if (input.inputs["left"]) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;*/
        move = transform.forward * input.moveAxis.y + transform.right * input.moveAxis.x;

        if (rb == null)
            return;

        if (playerStatus != null && playerStatus.isDead)
            return;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        rb.MovePosition(
            rb.position + move * PmoveSpeed * input.gameDeltaTime
        );

        //transform.position += move;
        if(input.IsReplay || status.isDead)
        {
            // リプレイ中は時間の調整を行わない
            return;
        }

        if (move.sqrMagnitude > 0f|| NormalTimer > 0f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0.05f;

        if (NormalTimer > 0f)
            NormalTimer -= Time.unscaledDeltaTime;
    }

    void FixedUpdate()
    {
        
    }

    public void OnShoot()
    {
        if(input.IsReplay) return;
        NormalTimer = NormalDuration;
        Time.timeScale = 1f;
    }
}
