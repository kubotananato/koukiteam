using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;           // 敵の移動速度
    public Transform player;           // プレイヤーを追跡させたい場合

    void Update()
    {
        // プレイヤーに向かって移動する例
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // 高さは固定

            // 【重要】Time.deltaTimeを掛けることで、タイムスケール（スロー・停止）が自動反映されます
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // プレイヤーの方向を向く
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
            }
        }
    }
}