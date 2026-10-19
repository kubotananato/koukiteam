using UnityEngine;

public class GameClear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject sceneManegerObj;
    private SceneCenge sceneChangeScript;
    // Update is called once per frame

    [SerializeField] private CanvasGroup targetCanvasGroup; // 操作したいCanvasのCanvasGroup

    void Start()
    {
        sceneManegerObj = GameObject.Find("SceneManeger");
        if (sceneManegerObj != null)
        {
            sceneChangeScript = sceneManegerObj.GetComponent<SceneCenge>();
        }
        else
        {
            Debug.LogWarning("SceneManagerが設定されていません。");
        }
    }
    private void FixedUpdate()
    {
        GameManeger.Instance.clearTime++;
    }

    void Update()
    {
        /*
        if (GameManeger.Instance.downEnemy >= 4)
        {
            if (!isGameClear)
            {
                sceneChangeScript.LoadResultScene();
            }
            isGameClear = true;

        }
        */
    }
    public void ToggleCanvas()
    {
        if (targetCanvasGroup == null) return;

        if (targetCanvasGroup.alpha == 0)
        {
            Show();
        }
       
    }


    public void Show()
    {
        if (targetCanvasGroup == null) return;

        targetCanvasGroup.alpha = 1f;             // 不透明
        targetCanvasGroup.interactable = true;    // 操作
        targetCanvasGroup.blocksRaycasts = true;  // クリックや入力

        targetCanvasGroup.transform.SetAsLastSibling();
    }
}
