using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class EnemyManagerKari : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;
    [System.Serializable]public struct eneConfig
    {
        public Vector3 _initPos;
        public Vector3 _targetPos; 
         
        public bool _isStayPos; 
        public bool _isGun;
        public int wave;
    }
    int currentWave = 0;
    int maxWaveNum = 0;
    public bool isFinishAllWave;
    int[] waveMaxEnemyNum;
    [SerializeField] List<eneConfig> enemys;
    EnemyConfig config;
    ReplayTimer repT;
    List<GameObject> enemyList = new List<GameObject>();
    void Start()
    {
        repT = GameObject.Find("ReplayManager").GetComponent<ReplayTimer>();
        SpawnEnemy();
        currentWave = 0;
        for(int i = 0; i < enemys.Count(); ++i)
        {
            maxWaveNum = Mathf.Max(maxWaveNum, enemys[i].wave);
        }

        // waveごとの必要エネミー破壊数を取得
        waveMaxEnemyNum = new int[maxWaveNum+1];
        // エネミーの数分for文まわして、ウェーブごとに何体倒せばいいかを定義。
        for(int i = 0; i < enemys.Count(); ++i)
        {
            if(enemys[i].wave > maxWaveNum) continue;
            waveMaxEnemyNum[enemys[i].wave]++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (repT.IsStartReplay())
        {
            DestroyEnemy();
            currentWave = 0;
            SpawnEnemy();
        }

        if(CheckFinishWave())
        {
            currentWave++;
            if(currentWave > maxWaveNum)
            {
                isFinishAllWave = true;
                currentWave = maxWaveNum;
                return;
            }

            SpawnEnemy();
        }
    }

    private bool CheckFinishWave()
    {
        // 破壊された数を記録
        int destroyNum = 0;
        for (int i = 0; i < enemyList.Count(); ++i)
        {
            if(enemyList[i] == null) 
            {
                destroyNum++;
                continue;
            }
            if (enemyList[i].GetComponent<EnemyHP>().isDead == false) continue;
            destroyNum++;
        }
        // 敵を必要数倒したら、次のウェーブに移る
        if (destroyNum >= waveMaxEnemyNum[currentWave])
        {
            Debug.Log("Wave終了!");
            return true;
        }

        return false;
    }

    void SpawnEnemy()
    {
        // 現在の敵オブジェクトは一応手放しておく(数に誤りが出るため)
        enemyList.Clear();

        for(int i = 0; i < enemys.Count(); ++i)
        {
            if(currentWave != enemys[i].wave) continue;
            SpawnOneEne(enemys[i]);
        }
    }

    void SpawnOneEne(eneConfig _config)
    {
        enemyList.Add(Instantiate(enemyPrefab));
        // 初期値の設定
        enemyList.Last().transform.position = _config._initPos;
        config = enemyList.Last().GetComponent<EnemyConfig>();
        config.isStayPosition = _config._isStayPos;
        config.targetPosition = _config._targetPos;
        config.isGun = _config._isGun;
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
