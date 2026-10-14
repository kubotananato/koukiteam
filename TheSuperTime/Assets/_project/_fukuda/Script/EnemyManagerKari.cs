using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class EnemyManagerKari : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;
    EnemyConfig config;
    ReplayTimer repT;
    List<GameObject> enemyList = new List<GameObject>();
    void Start()
    {
        repT = GameObject.Find("ReplayManager").GetComponent<ReplayTimer>();
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(repT.frame == 0 && repT.isReplay)
        {
            DestroyEnemy();
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        SpawnOneEne(new Vector3(-0.76f, 0.0f, -0.77f), new Vector3(11.41f, 0.82f,6.15f), false, true);
        SpawnOneEne(new Vector3(10.59f, 0.0f, 1.53f), new Vector3(4.0f, 1.0f,1.53f), false, true);
        SpawnOneEne(new Vector3(-8.33f, 0.0f, 5.25f), new Vector3(-1.28f, 0.38f,3.65f), false, true);
    }

    void SpawnOneEne(Vector3 _initPos, Vector3 _targetPos, bool _isStayPos, bool _isGun)
    {
        enemyList.Add(Instantiate(enemyPrefab));
        // 初期値の設定
        enemyList.Last().transform.position = _initPos;
        config = enemyList.Last().GetComponent<EnemyConfig>();
        config.isStayPosition = _isStayPos;
        config.targetPosition = _targetPos;
        config.isGun = _isGun;
    }

    void DestroyEnemy()
    {
        for(int i = 0; i < enemyList.Count(); ++i)
        {
            Destroy(enemyList[i]);
        }
        enemyList.Clear();
    }
}
