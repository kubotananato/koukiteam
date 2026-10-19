
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 敵の動き
// 指定の位置まで移動する
// 移動しながら撃つor撃たない
// 移動中に射程範囲内にプレイヤーが入ってきたら止まるor止まらない（後から）

public class EnemyMove : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject playerObj;
    Transform playerTransform;
    Vector3 targetPosition = Vector3.zero;
    const float viewDistance = 1000000f;
    float shotReach = 10.0f;
    float punchReach = 0.0f;
    LayerMask sightMask;
    bool isStayPosition;
    // 一度でも目標地点に到達したかどうか
    bool isReachedTarget = false;
    EnemyShot eneshot;
    EnemyConfig config;
    EnemyHP hp;
    EnemyAnimation anim;
    

    
    void Start()
    {
        eneshot = this.GetComponent<EnemyShot>();
        config = this.GetComponent<EnemyConfig>();
        hp = this.GetComponent<EnemyHP>();
        anim = this.GetComponent<EnemyAnimation>();

        playerObj = GameObject.Find("Player");

        SetValue();

        // 最初は指定地点まで移動する
        agent.stoppingDistance = 0f;
        agent.SetDestination(targetPosition);

        if(isStayPosition)
        {
            isReachedTarget = true;
        }
    }

    void SetValue()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = config.moveSpeed;
        agent.updateRotation = false; // 移動方向への自動回転を無効にする
        playerTransform = playerObj.transform;
        targetPosition = config.targetPosition;
        sightMask = config.sightMask;
        shotReach = config.shotReach;
        punchReach = config.punchReach;
        isStayPosition = config.isStayPosition;
    }

    void FixedUpdate()
    {
        // プレイヤーの向きに回転
        RotationToPlayer();

        // ターゲットへ動く
        MoveToTarget();

        // アニメーション用に歩く方向をセットする
        SetMoveDir();

        if(!config.isGun)
        {
            anim.SetIsNotGun();
            config.gunObj.SetActive(false);
        }
        else config.gunObj.SetActive(true);
    }

    void SetMoveDir()
    {
        // 死んでるなら適当に値を入れておく（いちおう）
        if(hp.isDead)
        {
            anim.SetMoveDir(10);
            return;
        }

        Vector3 velocity = agent.velocity;
        velocity.y = 0f;

    // 停止中、またはほとんど移動していない場合はIdle
    if (agent.isStopped || velocity.sqrMagnitude < 0.01f)
    {
        anim.SetMoveDir(5);
        return;
    }

    // ワールドの移動方向を、敵自身を基準にした方向へ変換
    Vector3 localDir = transform.InverseTransformDirection(velocity);

    // 前を0度として、右が90度、左が-90度
    float angle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

    // 45度ずつの8方向に分類する
    // 0:前 1:右前 2:右 3:右後 4:後 5:左後 6:左 7:左前
    int sector = Mathf.RoundToInt(angle / 45f);
    sector = (sector + 8) % 8;

    int moveDir = 5;

    switch (sector)
    {
        case 0: moveDir = 2; break; // 前
        case 1: moveDir = 3; break; // 右前
        case 2: moveDir = 6; break; // 右
        case 3: moveDir = 9; break; // 右後
        case 4: moveDir = 8; break; // 後
        case 5: moveDir = 7; break; // 左後
        case 6: moveDir = 4; break; // 左
        case 7: moveDir = 1; break; // 左前
    }

    anim.SetMoveDir(moveDir);
    }

    void RotationToPlayer()
    {
        if(hp.isDead)
        {
            return;
        }

        // プレイヤーの位置を向くようにする
        if(playerTransform != null)
        {
            Vector3 dir = playerTransform.position - transform.position;
            dir.y = 0.0f; // XZ軸のみで回転を行う

            if(dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }


    void MoveToTarget()
    {
        if(hp.isDead)
        {
            agent.isStopped = true;
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
        bool isInReach = config.isGun ? distance <= shotReach : distance <= punchReach;

        if(CanSeeTarget() && isInReach)
        {
            // プレイヤーが見える位置にいるなら、その場で止まって射撃を行う
            agent.isStopped = true;
            agent.updateRotation = false;

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
        if (Physics.Raycast(eyePosition, (playerTransform.position - eyePosition).normalized, out RaycastHit hit, viewDistance, sightMask))
        {
           // 最初に当たったものがターゲットであれば見えている
            return hit.transform == playerTransform;
        }

        return false;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public void DropItem()
    {
        config.isGun = false;
        anim.SetIsDamaged(true);
        // 武器を生み出してプレイヤー方向に飛ばす
        GameObject obj = Instantiate(config.dropItemPrefab);
        obj.transform.position = config.gunObj.transform.position;
        config.gunObj.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Dictionary<EneType, string> types = new Dictionary<EneType, string>()
        {
          {EneType.Gun, "PlayerBullet"},
            {EneType.Punch, "PlayerPunch"},
            {EneType.Throw, "PlayerThrow"}
        };
        string damageTag = types[config.damageType];

        // ダメージを受けるとき
        if (collision.gameObject.CompareTag(damageTag))
        {
            hp.Damaged();
        }

        // ダメージを受けたとき、武器を落とす
        for(int i = 0; i < 3; ++i)
        {
            EneType tmp = (EneType)i;
            if (collision.gameObject.CompareTag(types[tmp]) && config.isGun)
            {
                DropItem();
            }
        }

        

        /*if (collision.gameObject.CompareTag("PlayerPunch"))
        {
            if(config.isGun)
            {
                DropItem();
                return;
            }
            else hp.Damaged();
        }*/
        

        

    }

}
