using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    // 敵の設定を全部かき集めた
    [Header("設定")]
    [SerializeField] Transform playerTransform;

    [Header("移動")]
    [SerializeField] float moveSpeed = 0.0f;
    [SerializeField] Vector3 targetPos = Vector3.zero;
}
