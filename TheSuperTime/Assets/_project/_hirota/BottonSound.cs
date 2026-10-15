using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonSeClip;
    [Tooltip("銃の射撃音")]
    [SerializeField] private AudioClip gunShotClip;

    [Header("Audio Sources")]
    [Tooltip("効果音（SE）用のAudioSource")]
    [SerializeField] private AudioSource seAudioSource;
    [Tooltip("BGM用のAudioSource")]
    [SerializeField] private AudioSource bgmAudioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSourceがアサインされていなければ自動で取得・追加する
        if (seAudioSource == null)
        {
            seAudioSource = GetComponent<AudioSource>();
            if (seAudioSource == null)
            {
                seAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // BGM用のAudioSourceがなければ追加する
        if (bgmAudioSource == null)
        {
            // 同じオブジェクトに2つ目のAudioSourceを追加
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length > 1)
            {
                bgmAudioSource = sources[1];
            }
            else
            {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
            }
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
        RegisterButtonsInScene();
    }

    // シーン内にあるすべてのボタンを自動で探して登録する
    public void RegisterButtonsInScene()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonSE);
            button.onClick.AddListener(PlayButtonSE);
        }
    }

    // --- 効果音（SE）関連 ---

    public void PlayButtonSE()
    {
        PlaySE(buttonSeClip);
    }

    /// <summary>
    /// 任意の効果音を再生する汎用メソッド
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if (clip != null && seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 銃の射撃音を再生する例
    /// </summary>
    public void PlayGunShot()
    {
        PlaySE(gunShotClip);
    }

    // --- BGM関連 ---

    /// <summary>
    /// BGMを再生・変更する（ループ再生）
    /// </summary>
    /// <param name="bgmClip">流したいBGMのオーディオクリップ</param>
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null || bgmAudioSource == null) return;

        // すでに同じBGMが流れている場合は何もしない
        if (bgmAudioSource.clip == bgmClip && bgmAudioSource.isPlaying) return;

        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.loop = true; // BGMは基本ループ
        bgmAudioSource.Play();
    }

    /// <summary>
    /// BGMを停止する
    /// </summary>
    public void StopBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }
    }
}