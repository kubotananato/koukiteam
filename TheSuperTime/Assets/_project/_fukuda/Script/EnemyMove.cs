using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 敵の動き
// 指定の位置まで移動する
// 移動しながら撃つor撃たない
// 移動中に射程範囲内にプレイヤーが入ってきたら止まるor止まらない（後から）

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform selfTransform;
    [SerializeField] Transform targetTransform;

    [SerializeField] Vector3 targetPosition = Vector3.zero;
    EnemyConfig config = null;

    void Start()
    {
        // Configを取得
        config = this.GetComponent<EnemyConfig>();

        // 目標地点と自分の座標の高さを合わせる（高さは変更しないため。）
        targetPosition.y = transform.position.y;
    }

    void FixedUpdate()
    {
        // プレイヤーを見る
        selfTransform.LookAt(targetTransform);
        // 指定位置までの移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5.0f * Time.deltaTime);
    }
}
