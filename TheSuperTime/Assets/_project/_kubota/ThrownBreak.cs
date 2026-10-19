using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public GameObject breakEffect;
    public float safeTime = 0.05f;
    public float maxLifeTime = 10f;

    private float spawnTime;
    private bool broken = false;

    void Start()
    {
        spawnTime = Time.unscaledTime;
        Destroy(gameObject, maxLifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (broken) return;

        if (Time.unscaledTime - spawnTime < safeTime) return;
        if (collision.gameObject.CompareTag("Player")) return;

        Break();
    }

    void Break()
    {
        broken = true;

        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}