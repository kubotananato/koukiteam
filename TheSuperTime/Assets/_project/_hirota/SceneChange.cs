using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCenge : MonoBehaviour
{

    [SerializeField]private string targetScene;


    void Start()
    {
    }
    void Update()
    {
    }

    void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.LogWarning("シーンが正しく設定されてない");
        }
    }



}
