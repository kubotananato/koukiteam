using UnityEngine;

public class ClearCheck : MonoBehaviour
{
    EnemyManagerKari ene;
    public bool isClear;
    void Start()
    {
        ene = GameObject.Find("EnemyManager").GetComponent<EnemyManagerKari>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ene.isFinishAllWave)
        {
            isClear = true;
        }

        if(isClear)
        {
            Cursor.visible = true;

        // マウスカーソルのロックを解除する（画面中央に固定されていたものを自由にする）
        Cursor.lockState = CursorLockMode.None;
        }
    }
}
