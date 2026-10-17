using UnityEngine;

public class ClearCheck : MonoBehaviour
{
    [SerializeField] int enemyMax = 1;
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
    }
}
