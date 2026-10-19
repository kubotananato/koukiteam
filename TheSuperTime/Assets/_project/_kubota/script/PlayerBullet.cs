using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float PbulletSpeed = 10f;
    public float PbulletTime = 3.0f;

    [SerializeField] GameObject hitEffect;

    private TrailRenderer trail;
    private float elapsedTime = 0f;
    private bool isFinished = false;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (isFinished) return;

        transform.position +=
            transform.forward * PbulletSpeed * Time.deltaTime;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= PbulletTime)
        {
            FinishBullet();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFinished) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitEffect != null)
            {
                Instantiate(
                    hitEffect,
                    transform.position,
                    Quaternion.identity
                );
            }
        }

        // 敵に当たったら、レティクルを赤くする
        EnemyHP enemy = collision.collider.GetComponentInParent<EnemyHP>();

        if (enemy != null && !enemy.isDead)
        {
            if (ReticleHit.Instance != null)
            {
                ReticleHit.Instance.Hit();
            }
        }

        FinishBullet();
    }

    void FinishBullet()
    {
        if (isFinished) return;

        isFinished = true;

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r is TrailRenderer) continue;
            r.enabled = false;
        }

        float waitTime = 0f;

        if (trail != null)
        {
            trail.emitting = false;
            waitTime = trail.time;
        }

        Destroy(gameObject, waitTime);
    }
}
