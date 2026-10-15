using UnityEngine;

[DefaultExecutionOrder(99)]
public class ReplayTimer : MonoBehaviour
{
    ClearCheck clear;
    Input playerInput;

    public int frame = 0;
    public bool isReplay = false;

    void Start()
    {
        clear = GetComponent<ClearCheck>();

        playerInput = GameObject.Find("Player")
            .GetComponent<Input>();
    }

    void LateUpdate()
    {
        // クリアしたらリプレイ開始
        if (clear.isClear && !isReplay)
        {
            frame = 0;
            isReplay = true;
            Time.timeScale = 1f;
            return;
        }

        if (!isReplay) return;

        // 最後まで再生したら次のフレームで再スタート
        if (playerInput.replayFinished)
        {
            playerInput.RestartReplay();
        }
    }
}