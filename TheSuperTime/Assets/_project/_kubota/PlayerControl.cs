
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public float PmoveSpeed = 5f;

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;

    private Rigidbody rb;
    private float horizontal;
    private float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("PlayerにRigidbodyがありません！");
        }
    }

    void Update()
    {
        if (playerStatus != null && playerStatus.isDead)
            return;

        horizontal = 0f;
        vertical = 0f;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;

        if (horizontal != 0f || vertical != 0f || NormalTimer > 0f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0.05f;

        if (NormalTimer > 0f)
            NormalTimer -= Time.unscaledDeltaTime;
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        if (playerStatus != null && playerStatus.isDead)
            return;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * vertical + right * horizontal;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        rb.MovePosition(
            rb.position + move * PmoveSpeed * Time.fixedDeltaTime
        );
    }

    public void OnShoot()
    {
        NormalTimer = NormalDuration;
        Time.timeScale = 1f;
    }
}
