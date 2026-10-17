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
        lifeTime -= Time.unscaledDeltaTime;

        if(lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();

        if(enemy == null || enemy.isDead) return;

        if (!hitEnemies.Add(enemy)) return;

        enemy.Damaged();

        if (onHitEnemy != null) onHitEnemy(enemy);
    }
}