using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("説明書のUI")]
    [SerializeField] private CanvasGroup targetCanvasGroup;
    [SerializeField] private GameObject haikei;     // 説明書全体の親
    [SerializeField] private GameObject[] haikeis;  // 表示する順に登録
    [SerializeField] private GameObject button;     // Next / Back ボタンの親

    // buttons の直下にある各ボタンから、自動で子のTMPを取得する。
    private TMP_Text backButtonText;
    private TMP_Text nextButtonText;

    [Header("ウィンドウを開く演出")]
    [SerializeField, Min(0f)] private float openDuration = 0.25f;
    [SerializeField, Range(0.01f, 1f)] private float startScale = 0.75f;
    [SerializeField] private Vector2 stackOffset = new Vector2(24f, -24f);

    [Header("ウィンドウを閉じる演出")]
    [SerializeField, Min(0f)] private float closeDuration = 0.3f;
    [SerializeField, Range(0.001f, 1f)] private float closeScale = 0.03f;
    [Tooltip("未設定なら親UIの下中央へ縮小します。同じCanvas内のUIを指定してください。")]
    [SerializeField] private RectTransform minimizeTarget;

    private RectTransform[] windows;
    private Vector3[] initialPositions;
    private Vector3[] initialScales;
    private int currentIdx = -1;
    private bool initialized;

    private RectTransform openingWindow;
    private Vector3 openingTargetScale;
    private Vector3 openingStartScale;
    private float openingElapsed;

    private bool isClosing;
    private bool closingAll;
    private int closingFirstIndex;
    private float closingElapsed;
    private Vector3[] closingStartScales;
    private Vector3[] closingStartPositions;
    private Vector3[] closingTargetPositions;

    [SerializeField]GameObject hideButtonObj;

    private void Awake()
    {
        Initialize();
        HideImmediately();
    }

    private void Start()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM1();
    }

    private void Update()
    {
        if (isClosing)
        {
            UpdateClosing();
            return;
        }

        if (openingWindow == null) return;

        // ゲームがスロー・停止中でも、UIは通常速度で開く。
        openingElapsed += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(openingElapsed / Mathf.Max(openDuration, 0.0001f));
        float eased = 1f - Mathf.Pow(1f - t, 3f);
        openingWindow.localScale = Vector3.LerpUnclamped(
            openingStartScale, openingTargetScale, eased);

        if (t >= 1f) CompleteOpening();
    }

    private void OnDisable()
    {
        // 演出中に外部から無効化された場合も、次回表示へ状態を残さない。
        if (isClosing)
        {
            HideImmediately();
            return;
        }

        CompleteOpening();
    }

    private bool Initialize()
    {
        if (initialized) return true;

        if (haikeis == null || haikeis.Length == 0)
        {
            Debug.LogError("CanvasController: haikeis に説明書のUIを登録してください。", this);
            return false;
        }

        windows = new RectTransform[haikeis.Length];
        initialPositions = new Vector3[haikeis.Length];
        initialScales = new Vector3[haikeis.Length];
        closingStartScales = new Vector3[haikeis.Length];
        closingStartPositions = new Vector3[haikeis.Length];
        closingTargetPositions = new Vector3[haikeis.Length];

        for (int i = 0; i < haikeis.Length; i++)
        {
            if (haikeis[i] == null ||
                !haikeis[i].TryGetComponent<RectTransform>(out windows[i]))
            {
                Debug.LogError($"CanvasController: haikeis[{i}] にUIを登録してください。", this);
                return false;
            }

            initialPositions[i] = windows[i].anchoredPosition3D;
            initialScales[i] = windows[i].localScale;
        }

        initialized = true;
        return true;
    }

    public void ToggleCanvas()
    {
        if (isClosing) return;
        if (currentIdx >= 0) Hide();
        else Show();
    }

    public void Show()
    {
        if (isClosing) return;
        if (!Initialize()) return;

        // 再表示するたびに、必ず1枚目から始める。
        HideImmediately();
        SetVisibility(true);
        currentIdx = 0;
        OpenWindow(currentIdx);
        hideButtonObj.SetActive(false);
    }

    public void Hide()
    {
        if (isClosing) return;

        if (currentIdx < 0 || !initialized)
        {
            HideImmediately();
            return;
        }
        BeginClosing(true);
    }

    // 初期化・演出完了・外部からの無効化に使用する。
    private void HideImmediately()
    {
        openingWindow = null;
        isClosing = false;
        currentIdx = -1;

        if (haikeis != null)
        {
            for (int i = 0; i < haikeis.Length; i++)
            {
                if (haikeis[i] == null) continue;
                haikeis[i].SetActive(false);
                if (initialized) ResetWindow(i);
            }
        }

        SetVisibility(false);
        if (hideButtonObj != null)
        hideButtonObj.SetActive(true);
    }

    public void Next()
    {
        if (isClosing || currentIdx < 0) return;

        if (currentIdx >= haikeis.Length - 1)
        {
            Hide();
            return;
        }

        CompleteOpening();

        // それまでのウィンドウは表示したまま、次を手前に重ねる。
        currentIdx++;
        OpenWindow(currentIdx);
    }

    public void Back()
    {
        if (isClosing || currentIdx < 0) return;

        if (currentIdx == 0)
        {
            Hide();
            return;
        }

        BeginClosing(false);
    }

    private void BeginClosing(bool all)
    {
        // 開きかけでも、その時点の大きさから滑らかに縮小する。
        openingWindow = null;
        isClosing = true;
        closingAll = all;
        closingFirstIndex = all ? 0 : currentIdx;
        closingElapsed = 0f;

        for (int i = closingFirstIndex; i <= currentIdx; i++)
        {
            closingStartScales[i] = windows[i].localScale;
            closingStartPositions[i] = windows[i].position;
            closingTargetPositions[i] = GetMinimizePosition(windows[i]);
        }

        if (closeDuration <= 0f || !isActiveAndEnabled)
            FinishClosing();
    }

    private Vector3 GetMinimizePosition(RectTransform window)
    {
        if (minimizeTarget != null)
            return minimizeTarget.TransformPoint(minimizeTarget.rect.center);

        RectTransform parent = window.parent as RectTransform;
        if (parent != null)
        {
            // アンカーやCanvasの拡大率に依存せず、親UIの下中央へ移動する。
            return parent.TransformPoint(new Vector3(
                parent.rect.center.x, parent.rect.yMin, 0f));
        }

        return window.position;
    }

    private void UpdateClosing()
    {
        closingElapsed += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(closingElapsed / Mathf.Max(closeDuration, 0.0001f));
        float eased = t * t * (3f - 2f * t);

        for (int i = closingFirstIndex; i <= currentIdx; i++)
        {
            windows[i].localScale = Vector3.LerpUnclamped(
                closingStartScales[i], initialScales[i] * closeScale, eased);
            windows[i].position = Vector3.LerpUnclamped(
                closingStartPositions[i], closingTargetPositions[i], eased);
        }

        if (t >= 1f) FinishClosing();
    }

    private void FinishClosing()
    {
        if (closingAll)
        {
            // 全体の非表示は、縮小演出が終わってから行う。
            HideImmediately();
            return;
        }
        haikeis[currentIdx].SetActive(false);
        ResetWindow(currentIdx);
        currentIdx--;
        isClosing = false;
        UpdateButtonLabels();
    }

    private void OpenWindow(int index)
    {
        RectTransform window = windows[index];
        window.anchoredPosition3D = initialPositions[index] + (Vector3)(stackOffset * index);
        window.SetAsLastSibling();
        window.localScale = initialScales[index] * startScale;
        haikeis[index].SetActive(true);

        // ボタンの親は、各ウィンドウと同じ階層に配置する。
        if (button != null) button.transform.SetAsLastSibling();

        openingWindow = window;
        openingTargetScale = initialScales[index];
        openingStartScale = window.localScale;
        openingElapsed = 0f;

        if (openDuration <= 0f || !isActiveAndEnabled)
            CompleteOpening();

        UpdateButtonLabels();
    }

    private void UpdateButtonLabels()
    {
        if (button == null || currentIdx < 0) return;

        if (backButtonText == null)
        {
            Transform back = button.transform.Find("backButton");
            if (back != null)
                backButtonText = back.GetComponentInChildren<TMP_Text>(true);
        }

        if (nextButtonText == null)
        {
            Transform next = button.transform.Find("nextButton");
            if (next != null)
                nextButtonText = next.GetComponentInChildren<TMP_Text>(true);
        }

        if (backButtonText != null)
            backButtonText.text = currentIdx == 0 ? "CLOSE" : "Back";

        if (nextButtonText != null)
            nextButtonText.text = currentIdx == haikeis.Length - 1 ? "CLOSE" : "Next";
    }

    private void CompleteOpening()
    {
        // 連打や途中で閉じた場合も、小さいサイズのまま残さない。
        if (openingWindow != null)
            openingWindow.localScale = openingTargetScale;

        openingWindow = null;
    }

    private void ResetWindow(int index)
    {
        windows[index].anchoredPosition3D = initialPositions[index];
        windows[index].localScale = initialScales[index];
    }

    private void SetVisibility(bool visible)
    {
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.alpha = visible ? 1f : 0f;
            targetCanvasGroup.interactable = visible;
            targetCanvasGroup.blocksRaycasts = visible;
        }

        if (haikei != null) haikei.SetActive(visible);
        if (button != null)
        {
            button.SetActive(visible);
            if (visible && SoundManager.Instance != null)
            {
                Button[] childButtons = button.GetComponentsInChildren<Button>(true);
                foreach (var btn in childButtons)
                {
                    SoundManager.Instance.RegisterButton(btn);
                }
            }

        }
    }
}
