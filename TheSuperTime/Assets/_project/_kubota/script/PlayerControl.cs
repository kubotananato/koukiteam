using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public float PmoveSpeed = 5f;

    public float NormalTimer = 0f;
    public float NormalDuration = 0.5f;
    public float SEMoveInterval = 0.4f;
    private float SEMoveTimer = 0f;

    private Rigidbody rb;
    Input input;
    Vector3 move;
    PlayerStatus status;
    [SerializeField] SaveReplay saveReplay;
    Transform player;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Input playerInput;

    void Start()
    {
        player = this.transform;
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

        if (playerStatus.isDead)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopMoveSE();
            }

            return;
        }
        if (input.IsReplay || status.isDead)
        {
            Time.timeScale = 1.0f;
        }
        if (playerStatus != null && playerStatus.isDead)
            return;
        if (Keyboard.current == null)
            return;

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

        if (move.sqrMagnitude > 0f || NormalTimer > 0f)
        { 
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.05f;
        }

        // 足音の処理
        if (move.sqrMagnitude > 0.01f)
        {
            SEMoveTimer -= Time.deltaTime;

            if (SEMoveTimer <= 0f)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayerMoveSE();
                }

                SEMoveTimer = SEMoveInterval;
            }
        }
        else
        {
            // 止まったら、タイマーをリセットして、鳴っている足音も止める
            SEMoveTimer = 0f;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopMoveSE();
            }
        }

        if (NormalTimer > 0f)
            NormalTimer -= Time.unscaledDeltaTime;
    }

    void FixedUpdate()
    {
        
    }

    public void OnShoot()
    {
        if (input.IsReplay) return;
        NormalTimer = NormalDuration;
        Time.timeScale = 1f;
    }
}
