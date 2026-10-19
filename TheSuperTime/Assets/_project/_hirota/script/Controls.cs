using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup targetCanvasGroup; // 操作したいCanvasのCanvasGroup
    [SerializeField] GameObject haikei;
    [SerializeField] GameObject button;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image haikeiImage;
    int currentIdx = 0;


    void Start()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM1(); 
        }
        haikei.SetActive(false);
        button.SetActive(false);
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
        haikei.SetActive(true);
        button.SetActive(true);
        currentIdx = 0;

        /*targetCanvasGroup.alpha = 1f;             // 不透明
        targetCanvasGroup.interactable = true;    // 操作
        targetCanvasGroup.blocksRaycasts = true;  // クリックや入力

        targetCanvasGroup.transform.SetAsLastSibling();*/
    }

    // 非表示にする
    public void Hide()
    {
        if (targetCanvasGroup == null) return;

        haikei.SetActive(false);
        button.SetActive(false);

        /*targetCanvasGroup.alpha = 0f;             // 透明
        targetCanvasGroup.interactable = false;   // 操作無効化
        targetCanvasGroup.blocksRaycasts = false; // 入力を通さないように*/
    }

}