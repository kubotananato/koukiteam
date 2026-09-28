using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// 敵の動き
// 指定の位置まで移動する
// 移動しながら撃つor撃たない
// 移動中に射程範囲内にプレイヤーが入ってきたら止まるor止まらない（後から）

public class EnemyMove : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 targetPosition = Vector3.zero;
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float viewDistance = 10f;
    [SerializeField] LayerMask sightMask;
    // 一度でも目標地点に到達したかどうか
    bool isLandOnce = false;

    
    void Start()
    {

    }

    void FixedUpdate()
    {
        // 優先順位
        // 指定までの移動→Playerが見えていれば撃つ→見えなくなった時、見える位置までの移動（最終的には）

        // 目標地点と自分との距離
        float distance = Vector3.Distance(transform.position, targetPosition);
        // 目標地点に到達したかどうか
        bool isTargetPos = distance <= 0.5f;
        if(!isLandOnce)
        {
            if(isTargetPos)
            {
                isLandOnce = true;
                Debug.Log("toutatsu");
            }
            else
            {
                // 指定位置までの移動
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5.0f * Time.deltaTime);
            }
        }
        else
        {
            // 敵を生成。
            // 生成した時点で敵を追いかける敵か指定した座標まで移動して撃つ敵かというのをきめる。
            if (!CanSeeTarget())
            {
                agent.SetDestination(playerTransform.position);
            }
        }


        bool isReachShotRange = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
        if (isReachShotRange)
        {
            transform.LookAt(playerTransform);
        }
    }

    bool CanSeeTarget()
    {
        Vector3 toTarget = playerTransform.position - transform.position;

        // プレイヤーが敵の見える位置にいるかどうかを判定する。壁越しかどうかだけ
        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        if (Physics.Raycast(eyePosition, (playerTransform.position - eyePosition).normalized, out RaycastHit hit, viewDistance, sightMask))
        {
            // 最初に当たったものがターゲットであれば見えている
            return hit.transform == playerTransform;
        }
        return false;
    }
}
