using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCenge : MonoBehaviour
{

    public void LoadResultScene()
    {

        LoadScene("ResultScene");
    }

    public void LoadPreviousScene()
    {
        string Scene = SceneHistory.Instance.PreviousSceneName;
        GameManeger.Instance.ResetGameData();
        LoadScene(Scene);
    }

    public void LoadTargetScene(string Scene)
    {
        GameManeger.Instance.ResetGameData();
        LoadScene(Scene);
    }

    void LoadScene(string scene)
    {
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogWarning("シーンが正しく設定されてない");
        }
    }

}
