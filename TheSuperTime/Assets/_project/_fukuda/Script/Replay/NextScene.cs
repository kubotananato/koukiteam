using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    bool isRetry = false;
    bool isResult = false;
    bool isSelect = false;
    public bool next = false;
    int frame = 0;
    int nextDelay = 50;
    ReplayTimer repT;
    SceneCenge change;
    PlayerStatus status;
    bool once = false;
    bool audioOnce = false;
    void Start()
    {
        repT = GetComponent<ReplayTimer>();
        change = GameObject.Find("SceneManeger").GetComponent<SceneCenge>();
        status = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }
    void Update()
    {
        if(!repT.isReplay && !status.isDead) return;
        if(isRetry || isResult || isSelect) return;
        if(Keyboard.current.rKey.isPressed)
        {
            // リトライ
            isRetry = true;
        }
        if(Keyboard.current.eKey.isPressed)
        {
            if(status.isDead)
            {
                isSelect = true;
            }
            else
            isResult = true;
            if(!audioOnce)
            {
                SoundManager.Instance.PlaySEManaged(1, false);
                audioOnce = true;
            }
        }
    }
    void FixedUpdate()
    {
        if(!isRetry && !isResult && !isSelect) return;
        next = true;
        if(++frame >= nextDelay && !once)
        {
            if(isRetry) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //if(isResult) SceneManager.LoadScene(/*name*/);
            if(isResult) change.LoadResultScene();
            if(isSelect) 
            {
                Cursor.visible = true;

                // マウスカーソルのロックを解除する（画面中央に固定されていたものを自由にする）
                Cursor.lockState = CursorLockMode.None;
                change.LoadTargetScene("GameSelectScene");
            }
            SoundManager.Instance.StopSE(1);
            once = true;
        }
    }
}
