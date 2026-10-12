using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の生成直後に、NavMeshAgentをNavMesh上へ補正するコンポーネント。
/// スポナー側を変更できない場合に、敵のPrefabへ追加して使う。
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMeshSnapper : MonoBehaviour
{
    [SerializeField] private float _sampleRadius = 5f; // NavMeshを探す最大距離

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        // NavMeshAgentがNavMesh上に置けず、勝手に無効化されるのを防ぐため
        // いったん無効にしてから、補正後に有効化する
        _agent.enabled = false;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit,
                                   _sampleRadius, NavMesh.AllAreas))
        {
            // NavMesh上の位置へ移動してからAgentを有効化する
            transform.position = hit.position;
            _agent.enabled = true;
        }
        else
        {
            Debug.LogWarning($"{name}: 近くにNavMeshが見つかりません: {transform.position}", this);
        }
    }
}