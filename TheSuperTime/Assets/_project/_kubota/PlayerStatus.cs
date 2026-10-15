using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private GameObject sceneManegerObj;
    private SceneCenge sceneChangeScript;
    public float PlayerHp = 1;
    public bool isDead = false;

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
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isDead)
        {
            return;
        }

        Debug.Log("プレイヤーに何かがあった");
/*
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵に攻撃された");
            PlayerHp -= 1;

            if (PlayerHp <= 0)
            {
                isDead = true;
            }
        }
*/
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("敵に攻撃された");
            PlayerHp -= 1;
            PlayerHp = Mathf.Max(PlayerHp, 0);
            sceneChangeScript.LoadResultScene();

            if (PlayerHp <= 0)
            {
                isDead = true;
                Debug.Log("プレイヤー死亡！");
            }
        }
    }

}
