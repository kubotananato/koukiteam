using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip buttonSeClip;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーンが切り替わってもこのオブジェクトを破壊しないようにする
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // 初回起動時およびシーン切り替え時にボタンを自動登録する
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // イベントの登録解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが新しく読み込まれるたびに呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterButtonsInScene();
    }

    // シーン内にあるすべてのボタンを自動で探して登録する
    public void RegisterButtonsInScene()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var button in buttons)
        {
            // 多重登録を防ぐため一度クリアするか、そのまま追加する
            button.onClick.RemoveListener(PlayButtonSE);
            button.onClick.AddListener(PlayButtonSE);
        }
    }

    public void PlayButtonSE()
    {
        if (buttonSeClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonSeClip);
        }
    }
}