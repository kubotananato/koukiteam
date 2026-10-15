using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonSeClip;
    [Tooltip("テスト用の銃の射撃音")]
    [SerializeField] private AudioClip gunShotClip;

    [Header("BGM Settings (2 Types)")]
    [Tooltip("BGM 1（例：通常ステージ・タイトル用など）")]
    [SerializeField] private AudioClip bgm1;
    [Tooltip("BGM 2（例：バトル・ボス戦用など）")]
    [SerializeField] private AudioClip bgm2;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource seAudioSource;
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

  
    public void RegisterButtonsInScene()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (var button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonSE);
            button.onClick.AddListener(PlayButtonSE);
        }
    }

 

    public void PlayButtonSE()
    {
        PlaySE(buttonSeClip);
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip != null && seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayGunShot()
    {
        PlaySE(gunShotClip);
    }

 
    public void PlayBGM(int bgmIndex)
    {
        AudioClip targetClip = null;

        if (bgmIndex == 0) targetClip = bgm1;
        else if (bgmIndex == 1) targetClip = bgm2;

        PlayBGMClip(targetClip);
    }

    /// <summary>
    /// BGM1を再生するショートカット
    /// </summary>
    public void PlayBGM1()
    {
        PlayBGMClip(bgm1);
    }

    /// <summary>
    /// BGM2を再生するショートカット
    /// </summary>
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
}