using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵のAI。NavMeshでプレイヤーに近づき、攻撃範囲に入ったら止まって攻撃する。
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _target;              // 追いかける対象(空ならPlayerタグから探す)
    [SerializeField] private float _attackRange = 8f;        // この距離に入ったら止まって攻撃
    [SerializeField] private float _attackInterval = 2f;     // 攻撃の間隔(秒)
    [SerializeField] private float _repathInterval = 0.2f;   // 目的地を更新する間隔(秒)
    [SerializeField] private float _turnSpeed = 10f;         // 攻撃時の向き直りの速さ

    private NavMeshAgent _agent;
    private float _nextRepathTime;  // 次に経路を更新してよい時刻
    private float _nextAttackTime;  // 次に攻撃してよい時刻

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Prefabからはシーン上のオブジェクトを直接参照できないため、Playerタグで探す
        if (_target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _target = player.transform;
            }
        }
    }

    private void Update()
    {
        // 対象がいない、またはNavMesh上にいない場合は何もしない
        if (_target == null || !_agent.isOnNavMesh)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance > _attackRange)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    /// <summary>プレイヤーへ向かって移動する。</summary>
    private void ChaseTarget()
    {
        _agent.isStopped = false;

        // 毎フレーム経路計算すると重いので、一定間隔で更新する
        if (Time.time >= _nextRepathTime)
        {
            _agent.SetDestination(_target.position);
            _nextRepathTime = Time.time + _repathInterval;
        }
    }

    /// <summary>立ち止まってプレイヤーの方を向き、一定間隔で攻撃する。</summary>
    private void AttackTarget()
    {
        _agent.isStopped = true;

        // 水平方向だけプレイヤーのほうを向く
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, _turnSpeed * Time.deltaTime);
        }

        if (Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + _attackInterval;
        }
    }

    /// <summary>攻撃処理。今は仮でログのみ。弾の発射などをここに書く。</summary>
    private void Attack()
    {
        Debug.Log($"{name} が攻撃!");
    }
}