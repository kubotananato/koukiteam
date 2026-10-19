using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips (SE)")]
    [SerializeField] private AudioClip buttonSeClip; // 銃
    [SerializeField] private AudioClip PlayerMoveClip; // 足音
    [SerializeField] private AudioClip PlayerThrowClip; // 投げた時の音
    [SerializeField] private AudioClip PlayerPickupClip; // 銃を拾った時の音
    [SerializeField] private AudioClip PlayerPunchClip; // パンチの打撃音
    [Tooltip("インデックス（番号）で指定・管理するSEの配列")]
    [SerializeField] private AudioClip[] seClips;

    [Header("BGM Settings (2 Types)")]
    [Tooltip("BGM 1（例：通常ステージ・タイトル用など）")]
    [SerializeField] private AudioClip bgm1;
    [Tooltip("BGM 2（例：バトル・ボス戦用など）")]
    [SerializeField] private AudioClip bgm2;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource seAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [Header("SE Volume")]
    [SerializeField, Range(0f, 1f)] private float buttonSEVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float throwSEVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float pickupSEVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float punchSEVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float moveSEVolume = 0.3f;


    // SE管理用：ループ再生や個別の停止・一時停止に使うAudioSourceのリスト
    private List<AudioSource> activeSeSources = new List<AudioSource>();

    private AudioSource moveAudioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (seAudioSource == null)
        {
            seAudioSource = GetComponent<AudioSource>();
            if (seAudioSource == null)
            {
                seAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        if (bgmAudioSource == null)
        {
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

        // 足音専用の AudioSource を作る(銃などの音と分けるため)
        moveAudioSource = gameObject.AddComponent<AudioSource>();
        moveAudioSource.playOnAwake = false;
        moveAudioSource.volume = moveSEVolume;
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
        StopBGM();
        StopAllSE(); // シーン遷移時に制御中のSEをリセット
        StopMoveSE();
        RegisterButtonsInScene();
    }

    public void RegisterButtonsInScene()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonSE);
            button.onClick.AddListener(PlayButtonSE);
        }
    }

    // ==========================================
    // SE 制御機能（配列対応・ループ・個別管理）
    // ==========================================

    public void PlayButtonSE()
    {
        PlaySE(buttonSeClip, buttonSEVolume);
    }


    public void PlayerMoveSE() // 足音
    {
        PlayMove(PlayerMoveClip);
    }

    public void PlayerThrowSE()
    {
        PlaySE(PlayerThrowClip, throwSEVolume);
    }

    public void PlayerPickupSE()
    {
        PlaySE(PlayerPickupClip, pickupSEVolume);
    }

    public void PlayerPunchSE()
    {
        PlaySE(PlayerPunchClip, punchSEVolume);
    }

    /// <summary>
    /// 単発SE再生（重ねて鳴らすだけで制御不要な場合）
    /// </summary>
    public void PlaySE(AudioClip clip, float volume)
    {
        if (clip != null && seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip, volume);
        }
    }

    // 足音
    public void PlayMove(AudioClip clip)
    {
        if (clip == null || moveAudioSource == null) return;

        // すでに鳴っているときは、重ねない
        if (moveAudioSource.isPlaying) return;

        moveAudioSource.clip = clip;
        moveAudioSource.loop = false;
        moveAudioSource.Play();
    }

    // 足音を止める
    public void StopMoveSE()
    {
        if (moveAudioSource != null)
        {
            moveAudioSource.Stop();
        }
    }

    /// <summary>
    /// 配列のインデックス指定で単発SE再生
    /// </summary>
    public void PlaySENum(int clipIndex, float volume = 1f)
    {
        if (IsValidSeIndex(clipIndex))
        {
            PlaySE(seClips[clipIndex], volume);
        }
    }

    /// <summary>
    /// 管理可能（ループ指定・停止・一時停止対応）なSEを配列インデックス指定で再生
    /// </summary>
    public AudioSource PlaySEManaged(int clipIndex, bool loop = false)
    {
        if (!IsValidSeIndex(clipIndex)) return null;
        return PlaySEManaged(seClips[clipIndex], loop);
    }

    /// <summary>
    /// 管理可能なSEをAudioClip指定で再生
    /// </summary>
    public AudioSource PlaySEManaged(AudioClip clip, bool loop = false)
    {
        if (clip == null) return null;

        AudioSource source = GetFreeSeAudioSource();
        source.clip = clip;
        source.loop = loop;
        source.Play();

        return source;
    }

    /// <summary>
    /// 配列インデックス指定でSEを停止
    /// </summary>
    public void StopSE(int clipIndex)
    {
        if (!IsValidSeIndex(clipIndex)) return;
        StopSE(seClips[clipIndex]);
    }

    /// <summary>
    /// 特定のAudioClipを指定してSEを停止
    /// </summary>
    public void StopSE(AudioClip clip)
    {
        if (clip == null) return;

        foreach (var source in activeSeSources)
        {
            if (source.clip == clip)
            {
                source.Stop();
            }
        }
    }

    /// <summary>
    /// 配列インデックス指定でSEを一時停止
    /// </summary>
    public void PauseSE(int clipIndex)
    {
        if (!IsValidSeIndex(clipIndex)) return;

        foreach (var source in activeSeSources)
        {
            if (source.isPlaying && source.clip == seClips[clipIndex])
            {
                source.Pause();
            }
        }
    }

    /// <summary>
    /// 配列インデックス指定で一時停止中のSEを再開（スタート）
    /// </summary>
    public void ResumeSE(int clipIndex)
    {
        if (!IsValidSeIndex(clipIndex)) return;

        foreach (var source in activeSeSources)
        {
            if (source.clip == seClips[clipIndex])
            {
                source.UnPause();
            }
        }
    }

    /// <summary>
    /// 管理中のすべてのSEを停止
    /// </summary>
    public void StopAllSE()
    {
        foreach (var source in activeSeSources)
        {
            source.Stop();
        }
    }

    // ==========================================
    // BGM 制御機能（既存の仕様を維持）
    // ==========================================

    public void PlayBGM(int bgmIndex)
    {
        AudioClip targetClip = null;

        if (bgmIndex == 0) targetClip = bgm1;
        else if (bgmIndex == 1) targetClip = bgm2;

        PlayBGMClip(targetClip);
    }

    public void PlayBGM1()
    {
        PlayBGMClip(bgm1);
    }

    public void PlayBGM2()
    {
        PlayBGMClip(bgm2);
    }

    private void PlayBGMClip(AudioClip clip)
    {
        if (clip == null || bgmAudioSource == null) return;

        if (bgmAudioSource.clip == clip && bgmAudioSource.isPlaying) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }
    }

    // ==========================================
    // 内部補助処理
    // ==========================================

    private AudioSource GetFreeSeAudioSource()
    {
        // 停止中のAudioSourceがあれば再利用
        foreach (var source in activeSeSources)
        {
            if (!source.isPlaying) return source;
        }

        // なければ追加してリストで管理
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        activeSeSources.Add(newSource);
        return newSource;
    }

    private bool IsValidSeIndex(int index)
    {
        if (seClips == null || index < 0 || index >= seClips.Length)
        {
            Debug.LogWarning($"[SoundManager] SE配列のインデックスが無効です: {index}");
            return false;
        }
        return true;
    }
}