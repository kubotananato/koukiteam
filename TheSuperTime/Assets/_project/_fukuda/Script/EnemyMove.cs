using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

// 敵の動き
// 指定の位置まで移動する
// 移動しながら撃つor撃たない
// 移動中に射程範囲内にプレイヤーが入ってきたら止まるor止まらない（後から）

public class EnemyMove : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;
    Vector3 targetPosition = Vector3.zero;
    const float viewDistance = 1000000f;
    float shotReach = 10.0f;
    LayerMask[] sightMask;
    // 一度でも目標地点に到達したかどうか
    bool isReachedTarget = false;
    EnemyShot eneshot;
    EnemyConfig config;
    EnemyHP hp;

    
    void Start()
    {
        eneshot = this.GetComponent<EnemyShot>();
        config = this.GetComponent<EnemyConfig>();
        hp = this.GetComponent<EnemyHP>();

        SetValue();

        // 最初は指定地点まで移動する
        agent.stoppingDistance = 0f;
        agent.SetDestination(targetPosition);
    }

    void SetValue()
    {
        agent = config.agent;
        agent.speed = config.moveSpeed;
        playerTransform = config.playerTransform;
        targetPosition = config.targetPosition;
        sightMask = config.sightMask;
        shotReach = config.shotReach;

    }

    void FixedUpdate()
    {
        if(hp.isDead)
        {
            Destroy(gameObject);
            return;
        }


        // 優先順位
        // 指定までの移動→Playerが見えていれば撃つ→見えなくなった時、見える位置までの移動（最終的には）

        // 目標地点と自分との距離

        // 目標地点に到達したかどうか
        // 目標地点まで到達したら、移動ターゲットをプレイヤーの位置に変更
        // (プレイヤーが見えない位置にいる場合には見える位置まで移動するから)
        if (!isReachedTarget)
        {
            // 一つ目のターゲットまで移動する
            agent.SetDestination(targetPosition);

            // 目標地点まで到達したかどうか
            bool isTargetPos = !agent.pathPending && agent.hasPath && agent.remainingDistance <= 0.2f;
            if (isTargetPos)
            {
                isReachedTarget = true;
                Debug.Log("toutatu");
            }
        }

        if (!isReachedTarget) return;

        float distance = Vector3.Distance(transform.position, playerTransform.transform.position);
        bool isInReach = distance <= shotReach;

        if(CanSeeTarget() && isInReach)
        {
            // プレイヤーが見える位置にいるなら、その場で止まって向きだけ変える
            agent.isStopped = true;
            agent.updateRotation = false;

            Vector3 dir = playerTransform.position - transform.position;
            dir.y = 0f;
            if(dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }

            // 射撃可能にする
            eneshot.canShot = true;
        }
        else
        {
            // 見えなければプレイヤーを追う
            agent.updateRotation = true;
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }

    }

    bool CanSeeTarget()
    {
        Vector3 toTarget = playerTransform.position - transform.position;

        // プレイヤーが敵の見える位置にいるかどうかを判定する。壁越しかどうかだけ
        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        for(int i = 0; i < sightMask.Length; i++)
        {
            if (Physics.Raycast(eyePosition, (playerTransform.position - eyePosition).normalized, out RaycastHit hit, viewDistance, sightMask[0]))
            {
                // 最初に当たったものがターゲットであれば見えている
                return hit.transform == playerTransform;
            }
        }

        return false;
    }
}
