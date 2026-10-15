using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class Input : MonoBehaviour
{
    SaveReplay replay;
    ReplayTimer repT;

    public Dictionary<string, bool> inputs =
        new Dictionary<string, bool>
        {
            { "left", false },
            { "right", false },
            { "forward", false },
            { "back", false },
            { "shot", false },
            { "pick", false }
        };

    public Vector2 mouseDelta;
    public Vector2 moveAxis;
    public float gameDeltaTime;

    public bool IsReplay => repT != null && repT.isReplay;
    public bool replayFinished { get; private set; }
    public int replayVersion { get; private set; }

    Vector3 init;
    Quaternion initDir;

    bool wasReplay;
    int replayFrame;

    // 現在の記録を何秒分再生したか
    float usedTime;
    public float replaySpeed = 1.0f;

    void Start()
    {
        GameObject obj = GameObject.Find("ReplayManager");

        replay = obj.GetComponent<SaveReplay>();
        repT = obj.GetComponent<ReplayTimer>();

        init = transform.position;
        initDir = transform.rotation;
    }

    void Update()
    {
        if (IsReplay)
        {
            if (!wasReplay)
            {
                BeginReplay();
            }

            wasReplay = true;
            ReadReplay();
            return;
        }

        wasReplay = false;
        ReadLiveInput();
    }

    // 次のUpdateでリプレイを初期化する
    public void RestartReplay()
    {
        wasReplay = false;
    }

    // リプレイ開始時の初期化
    void BeginReplay()
    {
        replayVersion++;
        transform.position = init;
        transform.rotation = initDir;

        replayFrame = 0;
        usedTime = 0f;
        replayFinished = false;

        repT.frame = 0;
        Time.timeScale = 1f;
    }

    // 通常プレイの入力
    void ReadLiveInput()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;

        inputs["left"] =
            keyboard != null && keyboard.aKey.isPressed;

        inputs["right"] =
            keyboard != null && keyboard.dKey.isPressed;

        inputs["forward"] =
            keyboard != null && keyboard.wKey.isPressed;

        inputs["back"] =
            keyboard != null && keyboard.sKey.isPressed;

        bool pressed =
            mouse != null &&
            mouse.leftButton.wasPressedThisFrame;

        inputs["shot"] = pressed;
        inputs["pick"] = pressed;

        moveAxis = GetMoveAxis(inputs);

        mouseDelta = mouse != null
            ? mouse.delta.ReadValue()
            : Vector2.zero;

        gameDeltaTime = Time.deltaTime;
    }

    // リプレイの入力
    void ReadReplay()
    {
        ClearInput();

        float remaining = Time.unscaledDeltaTime;
        Vector2 totalMovement = Vector2.zero;

        int count = replay.GetInputsCount();

        while (remaining > 0f && replayFrame < count)
        {
            float duration = replay.GetDeltaTime(replayFrame);

            // 時間が進まない記録は読み飛ばす
            if (duration <= 0f)
            {
                replayFrame++;
                usedTime = 0f;
                continue;
            }

            Dictionary<string, bool> recorded =
                replay.GetInputs(replayFrame);

            float available = duration - usedTime;
            float step = Mathf.Min(remaining, available);

            // 記録を読み始めたときだけ発火させる
            if (usedTime == 0f)
            {
                inputs["shot"] |= recorded["shot"];
                inputs["pick"] |= recorded["pick"];
            }

            // 最後に読み込んだキー状態
            inputs["left"] = recorded["left"];
            inputs["right"] = recorded["right"];
            inputs["forward"] = recorded["forward"];
            inputs["back"] = recorded["back"];

            // 移動方向を時間に応じて合算
            totalMovement += GetMoveAxis(recorded) * step;

            // マウス移動量を読み込んだ割合に応じて合算
            mouseDelta +=
                replay.GetMouse(replayFrame) * (step / duration);

            gameDeltaTime += step;
            remaining -= step;

            if (step >= available)
            {
                replayFrame++;
                usedTime = 0f;
            }
            else
            {
                usedTime += step;
            }
        }

        if (gameDeltaTime > 0f)
        {
            moveAxis = totalMovement / gameDeltaTime;
        }

        replayFinished = replayFrame >= count;

        // 確認用のフレーム番号
        repT.frame = count > 0
            ? Mathf.Min(replayFrame, count - 1)
            : 0;
    }

    // キー状態から移動方向を作る
    Vector2 GetMoveAxis(Dictionary<string, bool> source)
    {
        float x = source["right"] ? 1f
                : source["left"] ? -1f : 0f;

        float y = source["forward"] ? 1f
                : source["back"] ? -1f : 0f;

        // 斜め移動が速くならないようにする
        return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
    }

    void ClearInput()
    {
        inputs["left"] = false;
        inputs["right"] = false;
        inputs["forward"] = false;
        inputs["back"] = false;
        inputs["shot"] = false;
        inputs["pick"] = false;

        moveAxis = Vector2.zero;
        mouseDelta = Vector2.zero;
        gameDeltaTime = 0f;
    }
}