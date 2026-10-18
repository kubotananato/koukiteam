using UnityEngine;
using System.Collections.Generic;

public class PunchHitbox : MonoBehaviour
{
    private float lifeTime;
    private System.Action<EnemyHP> onHitEnemy;

    private HashSet<EnemyHP> hitEnemies = new HashSet<EnemyHP>();

    public void Init(float life, System.Action<EnemyHP> callback)
    {
        lifeTime = life;
        onHitEnemy = callback;
    }

    void Update()
    {
        // スローの影響を受けない時間で、消えるまでを数える
        lifeTime -= Time.unscaledDeltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyHP enemy = collision.collider.GetComponentInParent<EnemyHP>();

        if (enemy == null || enemy.isDead)
        {
            return;
        }

        if (!hitEnemies.Add(enemy))
        {
            return;
        }

        // 当たったので、レティクルを赤くする
        if (onHitEnemy != null)
        {
            onHitEnemy(enemy);
        }
    }
}