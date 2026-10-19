using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCenge : MonoBehaviour
{
    public void LoadResultScene()
    {
        Cursor.visible = true;

        // マウスカーソルのロックを解除する（画面中央に固定されていたものを自由にする）
        Cursor.lockState = CursorLockMode.None;

        // フェードアウトを挟んでからシーンを読み込む
        StartFadeAndLoad("ResultScene");
    }
    
    public void ReLoadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadPreviousScene()
    {
        string Scene = SceneHistory.Instance.PreviousSceneName;
        GameManeger.Instance.ResetGameData();

        StartFadeAndLoad(Scene);
    }

    public void LoadTargetScene(string Scene)
    {
        GameManeger.Instance.ResetGameData();

        StartFadeAndLoad(Scene);
    }

    /// <summary>
    /// フェードアウトを実行し、暗転しきったあとにシーンをロードする中継メソッド
    /// </summary>
    private void StartFadeAndLoad(string scene)
    {
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogWarning("シーンが正しく設定されてない");
            return;
        }

        // ScreenFaderが存在すればフェードアウトを挟む
        if (ScreenFader.Instance != null)
        {
            ScreenFader.StartFadeOut(() => {
                // 暗転完了後に実際のシーン遷移を実行
                SceneManager.LoadScene(scene);
            });
        }
        else
        {
            //万が一Faderがない場合はそのままロード
            SceneManager.LoadScene(scene);
        }
    }
}