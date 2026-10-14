using UnityEngine;

public class ClearCheck : MonoBehaviour
{
    [SerializeField] int enemyMax = 1;
    int deadEnemyNum = 0;
    public bool isClear;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(deadEnemyNum >= enemyMax)
        {
            isClear = true;
        }
    }

    public void SetDeadEne()
    {
        deadEnemyNum++;
    }
}
