using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("画面全体を覆う黒いパネル（Image）")]
    [SerializeField] private Image fadePanel;

    [Header("Settings")]
    [Tooltip("フェードにかかる時間（秒）")]
    [SerializeField] private float defaultDuration = 0.8f;

    public bool IsFading { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadePanel == null)
        {
            fadePanel = GetComponentInChildren<Image>();
        }

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.enabled = true; // 起動時は表示しておく
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadePanel == null)
        {
            fadePanel = GetComponentInChildren<Image>();
        }

        // シーン切り替え時はパネルを有効化して真っ黒からスタート
        if (fadePanel != null)
        {
            fadePanel.enabled = true;
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
        }

        StartFadeIn();
    }

    public static void StartFadeOut(System.Action onCompleted = null)
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.FadeRoutine(true, Instance.defaultDuration, onCompleted));
        }
        else
        {
            onCompleted?.Invoke();
        }
    }

    public static void StartFadeIn(System.Action onCompleted = null)
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.FadeRoutine(false, Instance.defaultDuration, onCompleted));
        }
        else
        {
            onCompleted?.Invoke();
        }
    }

    private IEnumerator FadeRoutine(bool isFadeOut, float duration, System.Action onCompleted)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("FadePanel がアサインされていません。");
            IsFading = false;
            onCompleted?.Invoke();
            yield break;
        }

        // フェード開始時は必ずパネルを表示状態にする
        fadePanel.enabled = true;

        IsFading = true;
        float elapsed = 0f;
        Color c = fadePanel.color;

        float startAlpha = isFadeOut ? 0f : 1f;
        float endAlpha = isFadeOut ? 1f : 0f;

        while (elapsed < duration)
        {
            if (fadePanel == null) break;

            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadePanel.color = c;

            yield return null;
        }

        if (fadePanel != null)
        {
            c.a = endAlpha;
            fadePanel.color = c;

            // ★追加：フェードインが完了して透明度が0になったら、Imageを非表示にして入力を通すようにする
            if (!isFadeOut && Mathf.Approximately(endAlpha, 0f))
            {
                fadePanel.enabled = false;
            }
        }

        IsFading = false;
        onCompleted?.Invoke();
    }
}