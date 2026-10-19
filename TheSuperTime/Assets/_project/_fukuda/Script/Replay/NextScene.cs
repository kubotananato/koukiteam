using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    bool isRetry = false;
    bool isResult = false;
    public bool next = false;
    int frame = 0;
    int nextDelay = 50;
    ReplayTimer repT;
    SceneCenge change;
    PlayerStatus status;
    void Start()
    {
        repT = GetComponent<ReplayTimer>();
        change = GameObject.Find("SceneManeger").GetComponent<SceneCenge>();
        status = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }
    void Update()
    {
        if(!repT.isReplay && !status.isDead) return;
        if(isRetry || isResult) return;
        if(Keyboard.current.eKey.isPressed)
        {
            // リトライ
            isRetry = true;
        }
        if(Keyboard.current.rKey.isPressed)
        {
            isResult = true;
        }
    }
    void FixedUpdate()
    {
        if(!isRetry && !isResult) return;
        next = true;
        if(++frame >= nextDelay)
        {
            if(isRetry) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //if(isResult) SceneManager.LoadScene(/*name*/);
            if(isResult) change.LoadResultScene();
        }
    }
}
