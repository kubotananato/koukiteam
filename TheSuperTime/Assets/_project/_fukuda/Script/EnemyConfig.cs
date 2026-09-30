using UnityEngine;
using UnityEngine.AI;

public class EnemyConfig : MonoBehaviour
{
    // 敵の設定を全部かき集めた
    [Header("基本設定")]
    [SerializeField] public Transform playerTransform;
    [SerializeField] public NavMeshAgent agent;

    [Header("移動")]
    [SerializeField] public Vector3 targetPosition;
    [SerializeField] public float moveSpeed = 0.0f;

    [Header("視界など")]
    // プレイヤーと壁・障害物のみ入れる(弾・武器は入れない)
    [SerializeField] public LayerMask[] sightMask;


    [Header("攻撃")]
    [SerializeField] public float shotReach = 10.0f;
    [SerializeField] public int shotSpanFrame = 30; 
}
