using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup targetCanvasGroup; // 操作したいCanvasのCanvasGroup

    void Start()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM1(); 
        }
    }


    public void ToggleCanvas()
    {
        if (targetCanvasGroup == null) return;

        if (targetCanvasGroup.alpha == 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // 表示する
    public void Show()
    {
        if (targetCanvasGroup == null) return;

        targetCanvasGroup.alpha = 1f;             // 不透明
        targetCanvasGroup.interactable = true;    // 操作
        targetCanvasGroup.blocksRaycasts = true;  // クリックや入力

        targetCanvasGroup.transform.SetAsLastSibling();
    }

    // 非表示にする
    public void Hide()
    {
        if (targetCanvasGroup == null) return;

        targetCanvasGroup.alpha = 0f;             // 透明
        targetCanvasGroup.interactable = false;   // 操作無効化
        targetCanvasGroup.blocksRaycasts = false; // 入力を通さないように
    }
}