using UnityEngine;
using System.Collections.Generic;

public class PunchHitbox : MonoBehaviour
{
    private float lifeTime;
    private System.Action<EnemyHP, Vector3> onHitEnemy;

    private HashSet<EnemyHP> hitEnemies = new HashSet<EnemyHP>();

    public void Init(float life, System.Action<EnemyHP, Vector3> callback)
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

        Vector3 hitPoint = collision.GetContact(0).point;

        if (onHitEnemy != null)
        {
            onHitEnemy(enemy, hitPoint);
        }
    }
}