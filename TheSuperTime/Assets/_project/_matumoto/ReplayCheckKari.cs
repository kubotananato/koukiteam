using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayCheckKari : MonoBehaviour
{
    public bool goToReplay = false;
    // Update is called once per frame

    void Start()
    {
        
    }


    void Update()
    {
        if (GameManeger.Instance.downEnemy >= 4)
        {
            goToReplay = true;
            Debug.Log("リプレイへ行く");
        }
    }
}
