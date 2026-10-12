using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 倒された敵を、一定時間後に再生成する管理コンポーネント。
/// シーンに1つだけ置く。
/// </summary>
public class EnemyRespawnManager : MonoBehaviour
{
    // どこからでも参照できるようにする(シーンに1つだけの想定)
    public static EnemyRespawnManager Instance { get; private set; }

    [SerializeField] private GameObject _enemyPrefab;      // 再生成する敵のPrefab
    [SerializeField] private float _respawnDelay = 5f;     // 倒されてから再生成までの秒数
    [SerializeField] private float _navMeshSampleRadius = 5f; // NavMesh補正で探す最大距離

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        // 自分が破棄されるときは、参照を外す
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>敵が倒されたときに呼ばれ、再生成を予約する。</summary>
    public void RequestRespawn(Vector3 position, Quaternion rotation)
    {
        StartCoroutine(RespawnAfterDelay(position, rotation));
    }

    /// <summary>一定時間待ってから、敵を再生成する。</summary>
    private IEnumerator RespawnAfterDelay(Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(_respawnDelay);

        // NavMesh上の位置に補正する。見つからなければ、元の位置のまま生成する
        if (NavMesh.SamplePosition(position, out NavMeshHit hit,
                                   _navMeshSampleRadius, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            Debug.LogWarning($"リスポーン位置の近くにNavMeshがありません: {position}", this);
        }

        Instantiate(_enemyPrefab, position, rotation);
    }
}