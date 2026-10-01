using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject sceneManegerObj;
    private SceneCenge sceneChangeScript;
    // Update is called once per frame

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


    void Update()
    {
        if (GameManeger.Instance.downEnemy >= 4)
        {
            sceneChangeScript.LoadResultScene();
        }
    }
}
