using UnityEngine;
// 新しいInput Systemを使うための宣言
using UnityEngine.InputSystem;

public class SuperHotGameManager : MonoBehaviour
{
    [Header("=== プレイヤーの設定 ===")]
    public Transform playerTransform;     // プレイヤーのTransform
    public float playerMoveSpeed = 8f;    // プレイヤーの移動速度

    [Header("=== タイムスケール設定（敵・弾用） ===")]
    public float normalTimeScale = 1.0f;  // 動いているときの速さ
    public float stopTimeScale = 1.0f;   // 止まっているときの超スロー
    public float transitionSpeed = 5.0f;   // 時間変化の滑らかさ

    // プレイヤーが動いているかどうか（True / False）
    [HideInInspector]
    public bool isPlayerMoving = false;

    void Update()
    {
        // キーボードが接続されていない場合の対策
        if (Keyboard.current == null) return;

        // 1. 新しいInput Systemを使った入力判定（WASDキーや矢印キー）
        float h = 0f;
        float v = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;

        isPlayerMoving = (h != 0 || v != 0);

        // 2. プレイヤーの移動処理（Trueのときだけ動く。タイムスケールの影響を受けない）
        if (isPlayerMoving && playerTransform != null)
        {
            Vector3 moveDir = new Vector3(h, 0, v).normalized;
            playerTransform.Translate(moveDir * playerMoveSpeed * Time.deltaTime, Space.Self);
        }

        // 3. 敵・弾用のタイムスケール一括管理
        float targetScale = isPlayerMoving ? normalTimeScale : stopTimeScale;
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 物理演算の破綻を防ぐ
    }
}