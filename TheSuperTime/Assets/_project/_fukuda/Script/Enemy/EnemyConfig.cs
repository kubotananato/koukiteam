using UnityEngine;
using UnityEngine.AI;
public enum EneType
    {
        Gun,
        Throw,
        Punch
    };
public class EnemyConfig : MonoBehaviour
{
    [Header("タイプ")]
    
    [SerializeField]public EneType damageType = EneType.Gun;

    [Header("移動")]
    [SerializeField] public bool isStayPosition = false;
    [SerializeField] public Vector3 targetPosition;
    [SerializeField] public float moveSpeed = 0.0f;

    [Header("視界など")]
    // プレイヤーと壁・障害物のみ入れる(弾・武器は入れない)
    [SerializeField] public LayerMask sightMask;


    [Header("攻撃")]
    [SerializeField] public float punchReach = 5.0f;
    [SerializeField] public float shotReach = 10.0f;
    [SerializeField] public int shotSpanFrame = 30;
    [SerializeField] public bool isGun = false;
    [SerializeField] public GameObject gunObj;
    [Header("アイテム")]
    [SerializeField] public GameObject dropItemPrefab;
}
