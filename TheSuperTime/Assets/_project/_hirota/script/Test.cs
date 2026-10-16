using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    private GameObject ManegerObj;
    private GameClear sceneChangeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ManegerObj = GameObject.Find("SceneManeger");
        if (ManegerObj != null)
        {
            sceneChangeScript = ManegerObj.GetComponent<GameClear>();
        }
        else
        {
            Debug.LogWarning("SceneManagerが設定されていません。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            sceneChangeScript.ToggleCanvas();
        }
    }
}
