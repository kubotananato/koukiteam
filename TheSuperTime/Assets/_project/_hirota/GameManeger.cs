using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static GameManeger Instance { get; private set; }

    //持ち越したいデータ
    public float clearTime = 0.0f;
    public int downEnemy = 0;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ResetGameData()
    {
        clearTime = 0.0f;
        downEnemy = 0;
    }
}
