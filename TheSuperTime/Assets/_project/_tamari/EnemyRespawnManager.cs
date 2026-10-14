using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の生成と再生成を管理するコンポーネント。
/// 指定した固定座標から複数体を生成し、倒されたら生成元の座標に再生成する。
/// シーンに1つだけ置く。
/// </summary>
public class EnemyRespawnManager : MonoBehaviour
{
    // どこからでも参照できるようにする(シーンに1つだけの想定)
    public static EnemyRespawnManager Instance { get; private set; }

    /// <summary>1つの固定座標についての生成設定。</summary>
    [System.Serializable]
    public class SpawnEntry
    {
        [SerializeField] private Transform _point;          // 生成する座標(空のGameObject)
        [SerializeField] private int _count = 1;            // この座標から出す敵の数
        [SerializeField] private float _spawnInterval = 0.5f; // 1体ずつ出す間隔(秒)

        public Transform Point => _point;
        public int Count => _count;
        public float SpawnInterval => _spawnInterval;
    }

    [Header("共通設定")]
    [SerializeField] private GameObject _enemyPrefab;         // 生成する敵のPrefab
    [SerializeField] private float _respawnDelay = 5f;        // 倒されてから再生成までの秒数
    [SerializeField] private float _navMeshSampleRadius = 5f; // NavMesh補正で探す最大距離

    [Header("固定座標からの生成")]
    [SerializeField] private SpawnEntry[] _spawnEntries;      // 生成元の一覧
    [SerializeField] private bool _spawnOnStart = true;       // 開始時に自動で生成するか

    // 生成した敵のID(GetInstanceID)と、生成元の番号の対応表
    private readonly Dictionary<int, int> _enemyOrigins = new Dictionary<int, int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_spawnOnStart)
        {
            SpawnAll();
        }
    }

    private void OnDestroy()
    {
        // 自分が破棄されるときは、参照を外す
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>登録されたすべての固定座標から敵を生成する。</summary>
    public void SpawnAll()
    {
        if (_enemyPrefab == null || _spawnEntries == null)
        {
            Debug.LogError("敵のPrefabまたは生成元が設定されていません。", this);
            return;
        }

        for (int i = 0; i < _spawnEntries.Length; i++)
        {
            StartCoroutine(SpawnFromEntry(i));
        }
    }

    /// <summary>
    /// 敵が倒されたときに呼ぶ(推奨)。
    /// その敵の生成元の座標に、一定時間後に再生成する。
    /// </summary>
    public void RequestRespawn(GameObject defeatedEnemy)
    {
        int id = defeatedEnemy.GetInstanceID();
        if (!_enemyOrigins.TryGetValue(id, out int entryIndex))
        {
            Debug.LogWarning("生成元が不明な敵のため、再生成しません。", this);
            return;
        }

        _enemyOrigins.Remove(id);
        StartCoroutine(RespawnAfterDelay(entryIndex));
    }

    /// <summary>
    /// 既存の呼び出し用(互換)。
    /// 渡された位置に最も近い固定座標に再生成する。rotationは使わない。
    /// 敵が移動してから倒されると、別の座標から出ることがあるため、
    /// 可能なら RequestRespawn(GameObject) に切り替える。
    /// </summary>
    public void RequestRespawn(Vector3 position, Quaternion rotation)
    {
        int entryIndex = FindNearestEntryIndex(position);
        if (entryIndex < 0)
        {
            Debug.LogWarning("再生成できる固定座標がありません。", this);
            return;
        }

        StartCoroutine(RespawnAfterDelay(entryIndex));
    }

    /// <summary>1つの固定座標から、設定された数の敵を間隔を空けて生成する。</summary>
    private IEnumerator SpawnFromEntry(int entryIndex)
    {
        SpawnEntry entry = _spawnEntries[entryIndex];

        for (int i = 0; i < entry.Count; i++)
        {
            SpawnEnemy(entryIndex);

            // 最後の1体のあとは待たない
            if (entry.SpawnInterval > 0f && i < entry.Count - 1)
            {
                yield return new WaitForSeconds(entry.SpawnInterval);
            }
        }
    }

    /// <summary>一定時間待ってから、指定の生成元に敵を1体再生成する。</summary>
    private IEnumerator RespawnAfterDelay(int entryIndex)
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnEnemy(entryIndex);
    }

    /// <summary>指定の生成元の座標に敵を1体生成し、生成元を記録する。</summary>
    private void SpawnEnemy(int entryIndex)
    {
        Transform point = _spawnEntries[entryIndex].Point;
        if (point == null)
        {
            Debug.LogWarning($"生成元{entryIndex}の座標が未設定(None)です。", this);
            return;
        }

        // NavMesh上の位置に補正する
        // 見つからない場合は生成しない(NavMesh外に出すとNavMeshAgentのエラーになるため)
        if (!NavMesh.SamplePosition(point.position, out NavMeshHit hit,
                                    _navMeshSampleRadius, NavMesh.AllAreas))
        {
            Debug.LogWarning($"{point.name} の近くにNavMeshがありません。生成をスキップしました。", point);
            return;
        }

        GameObject enemy = Instantiate(_enemyPrefab, hit.position, point.rotation);

        // どの生成元から出た敵かを覚えておく(再生成先の決定に使う)
        _enemyOrigins[enemy.GetInstanceID()] = entryIndex;
    }

    /// <summary>指定位置に最も近い生成元の番号を返す。無ければ-1。</summary>
    private int FindNearestEntryIndex(Vector3 position)
    {
        int nearestIndex = -1;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < _spawnEntries.Length; i++)
        {
            Transform point = _spawnEntries[i].Point;
            if (point == null)
            {
                continue;
            }

            float distance = Vector3.Distance(position, point.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }
}