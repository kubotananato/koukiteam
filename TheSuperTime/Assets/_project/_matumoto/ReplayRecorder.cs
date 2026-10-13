using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct FrameInput
{
    public float time;
    public Vector3 inputDir;
    public Quaternion cameraRot;
    public bool isShooting;
}

public class ReplayRecorder : MonoBehaviour
{
    [Header("=== 参照 ===")]
    public Transform playerTransform;
    public Transform cameraTransform;

    [Header("=== リプレイ＆クリア条件 ===")]
    public int targetDefeatCount = 4; // 目標討伐数（4体）

    public List<FrameInput> recordedFrames = new List<FrameInput>();

    public enum State { Recording, Replaying, Idle }
    public State currentState = State.Idle;

    private int replayIndex = 0;

    // --- 【追加】初期地点（位置と回転）を記憶しておく変数 ---
    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;
    private Quaternion initialCameraRotation;

    void Start()
    {
        // ゲーム開始と同時に記録をスタート
        StartRecording();
    }

    public void StartRecording()
    {
        // --- 【追加】記録開始した瞬間の位置・回転を「初期地点」として保存しておく ---
        if (playerTransform != null)
        {
            initialPlayerPosition = playerTransform.position;
            initialPlayerRotation = playerTransform.rotation;
        }
        if (cameraTransform != null)
        {
            initialCameraRotation = cameraTransform.rotation;
        }

        recordedFrames.Clear();
        currentState = State.Recording;
        Debug.Log("入力を記録中...");
    }

    void Update()
    {
        // 1. 記録中の処理
        if (currentState == State.Recording)
        {
            // 敵が4体倒されたかチェック
            if (GameManeger.Instance != null && GameManeger.Instance.downEnemy >= targetDefeatCount)
            {
                Debug.Log("敵を4体撃破！リプレイに切り替えます。");
                StartReplay();
                return;
            }

            // プレイヤーの入力を記録
            FrameInput frame = new FrameInput();
            frame.time = Time.time;

            float h = 0f, v = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed) h -= 1f;
                if (Keyboard.current.dKey.isPressed) h += 1f;
                if (Keyboard.current.sKey.isPressed) v -= 1f;
                if (Keyboard.current.wKey.isPressed) v += 1f;
            }
            frame.inputDir = new Vector3(h, 0, v).normalized;

            if (cameraTransform != null)
            {
                frame.cameraRot = cameraTransform.rotation;
            }

            frame.isShooting = Mouse.current != null && Mouse.current.leftButton.isPressed;

            recordedFrames.Add(frame);
        }
        // 2. リプレイ再生中の処理
        else if (currentState == State.Replaying)
        {
            PlayReplayFrame();
        }
    }

    public void StartReplay()
    {
        currentState = State.Replaying;
        replayIndex = 0;

        // --- 【追加】リプレイが始まる直前に、プレイヤーを初期地点へ強制送還する ---
        if (playerTransform != null)
        {
            playerTransform.position = initialPlayerPosition;
            playerTransform.rotation = initialPlayerRotation;
        }
        if (cameraTransform != null)
        {
            cameraTransform.rotation = initialCameraRotation;
        }

        Debug.Log("リプレイ再生開始！ 初期位置に戻しました。 総フレーム数: " + recordedFrames.Count);
    }

    void PlayReplayFrame()
    {
        // リプレイデータの最後まで再生し終わったら
        if (replayIndex >= recordedFrames.Count)
        {
            Debug.Log("リプレイ終了！ クリア画面へ");
            currentState = State.Idle;
            return;
        }

        // 保存されたデータを読み出してプレイヤーを動かす
        FrameInput frame = recordedFrames[replayIndex];

        if (playerTransform != null)
        {
            playerTransform.Translate(frame.inputDir * 7f * Time.unscaledDeltaTime, Space.Self);
        }

        if (cameraTransform != null)
        {
            cameraTransform.rotation = frame.cameraRot;
        }

        replayIndex++;
    }
}