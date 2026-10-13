using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵を生成するスポナー。
/// 固定座標モードとランダムモードをInspectorで切り替えられる。
/// ランダムモードでは、NavMesh上の歩ける位置に補正できる。
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>生成モード。</summary>
    public enum SpawnMode
    {
        Fixed,  // あらかじめ置いた座標に生成
        Random  // 指定範囲内のランダム位置に生成
    }

    [Header("共通設定")]
    [SerializeField] private GameObject _enemyPrefab;       // 生成する敵のPrefab
    [SerializeField] private SpawnMode _spawnMode = SpawnMode.Fixed;
    [SerializeField] private bool _spawnOnStart = true;     // 開始時に自動生成するか

    [Header("固定座標モード")]
    [SerializeField] private Transform[] _fixedSpawnPoints; // 空のGameObjectを配置して登録

    [Header("ランダムモード")]
    [SerializeField] private int _randomSpawnCount = 5;     // 生成する数
    [SerializeField] private Vector3 _areaCenter = Vector3.zero;            // 生成範囲の中心
    [SerializeField] private Vector3 _areaSize = new Vector3(10f, 0f, 10f); // 生成範囲のサイズ
    [SerializeField] private Transform _player;             // プレイヤー(近すぎる生成を避ける用)
    [SerializeField] private float _minDistanceFromPlayer = 5f; // プレイヤーからの最低距離
    [SerializeField] private int _maxTryCount = 20;         // 位置決めの再試行回数

    [Header("NavMesh補正")]
    [SerializeField] private bool _snapToNavMesh = true;    // NavMesh上に補正するか
    [SerializeField] private float _navMeshSampleRadius = 2f; // 補正で探す最大距離

    // 生成した敵の一覧(全滅判定などに使える)
    private readonly List<GameObject> _spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        if (_spawnOnStart)
        {
            Spawn();
        }
    }

    /// <summary>現在のモードに従って敵を生成する。</summary>
    public void Spawn()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("敵のPrefabが設定されていません。", this);
            return;
        }

        switch (_spawnMode)
        {
            case SpawnMode.Fixed:
                SpawnAtFixedPoints();
                break;
            case SpawnMode.Random:
                SpawnAtRandomPositions();
                break;
        }
    }

    /// <summary>登録された固定座標すべてに敵を生成する。</summary>
    private void SpawnAtFixedPoints()
    {
        foreach (Transform point in _fixedSpawnPoints)
        {
            if (point == null)
            {
                continue;
            }
            // 位置と向きは空のGameObjectのものをそのまま使う
            SpawnEnemy(point.position, point.rotation);
        }
    }

    /// <summary>範囲内のランダムな位置に敵を指定数生成する。</summary>
    private void SpawnAtRandomPositions()
    {
        for (int i = 0; i < _randomSpawnCount; i++)
        {
            if (TryGetRandomPosition(out Vector3 position))
            {
                // 向きはY軸まわりのランダムな角度にする
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                SpawnEnemy(position, rotation);
            }
        }
    }

    /// <summary>
    /// 条件を満たすランダム位置を探す。
    /// 見つからなければfalseを返す。
    /// </summary>
    private bool TryGetRandomPosition(out Vector3 position)
    {
        for (int i = 0; i < _maxTryCount; i++)
        {
            // 範囲(直方体)の中から一様にランダムな点を選ぶ
            Vector3 candidate = _areaCenter + new Vector3(
                Random.Range(-_areaSize.x * 0.5f, _areaSize.x * 0.5f),
                Random.Range(-_areaSize.y * 0.5f, _areaSize.y * 0.5f),
                Random.Range(-_areaSize.z * 0.5f, _areaSize.z * 0.5f));

            // プレイヤーに近すぎる場合は次の候補へ
            if (_player != null &&
                Vector3.Distance(candidate, _player.position) < _minDistanceFromPlayer)
            {
                continue;
            }

            // NavMesh補正がオンなら、歩ける場所に補正できた場合のみ採用する
            if (_snapToNavMesh)
            {
                if (!TrySnapToNavMesh(candidate, out Vector3 snapped))
                {
                    continue;
                }
                candidate = snapped;
            }

            position = candidate;
            return true;
        }

        Debug.LogWarning("条件を満たす生成位置が見つかりませんでした。", this);
        position = Vector3.zero;
        return false;
    }

    /// <summary>
    /// 候補位置をNavMesh上の最も近い位置に補正する。
    /// NavMeshが無い、または近くに無い場合はfalseを返す。
    /// </summary>
    private bool TrySnapToNavMesh(Vector3 candidate, out Vector3 snapped)
    {
        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _navMeshSampleRadius, NavMesh.AllAreas))
        {
            snapped = hit.position;
            return true;
        }

        snapped = candidate;
        return false;
    }

    /// <summary>敵を1体生成してリストに記録する。</summary>
    private void SpawnEnemy(Vector3 position, Quaternion rotation)
    {
        GameObject enemy = Instantiate(_enemyPrefab, position, rotation);
        _spawnedEnemies.Add(enemy);
    }

    /// <summary>Sceneビューで生成範囲を確認するためのギズモ表示。</summary>
    private void OnDrawGizmosSelected()
    {
        if (_spawnMode != SpawnMode.Random)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_areaCenter, _areaSize);
    }
}