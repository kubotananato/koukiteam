using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    ///アクセスのためのインスタンス
    public static SceneHistory Instance { get; private set; }
    //前のシーンの名前を保存する
    public string PreviousSceneName { get; private set; } = "なし";
    //今のシーンの名前を保存する
    public string currentSceneName = "";

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //現在のシーン
        currentSceneName = SceneManager.GetActiveScene().name;

        //シーンが変わったときに呼ばれる関数を登録
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene Scene, LoadSceneMode mode)
    {
        //現在で保存していたものを前のシーンとして保存
        PreviousSceneName = currentSceneName;
        //現在のシーンを更新
        currentSceneName = Scene.name;

    }
}
