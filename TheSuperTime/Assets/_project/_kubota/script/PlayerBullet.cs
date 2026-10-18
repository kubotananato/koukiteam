using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
    public float PbulletSpeed = 10f;
    public float PbulletTime = 3.0f;

    [SerializeField] GameObject hitEffect;

    [Header("赤い弾道の設定")]
    public Material trailMaterial;
    public float trailWidth = 0.025f;
    public float trailDuration = 0.8f;

    private LineRenderer trailLine;
    private float elapsedTime = 0f;
    private bool isFinished = false;

    void Start()
    {
        // 弾道を表示するためのオブジェクトを作る
        GameObject trailObject = new GameObject("BulletTrail");

        // 弾本体とは別のオブジェクトにする
        trailLine = trailObject.AddComponent<LineRenderer>();

        trailLine.useWorldSpace = true;
        trailLine.positionCount = 1;
        trailLine.SetPosition(0, transform.position);

        trailLine.startWidth = trailWidth;
        trailLine.endWidth = trailWidth;

        trailLine.startColor = Color.red;
        trailLine.endColor = Color.red;

        if (trailMaterial != null)
        {
            trailLine.material = trailMaterial;
        }
        else
        {
            Shader shader = Shader.Find("Sprites/Default");

            if (shader != null)
            {
                trailLine.material = new Material(shader);
            }
        }
    }

    void Update()
    {
        if (isFinished) return;

        // 弾を前に進める
        transform.position +=
            transform.forward * PbulletSpeed * Time.deltaTime;

        // 弾が通った位置まで線を伸ばす
        if (trailLine != null)
        {
            trailLine.positionCount++;
            trailLine.SetPosition(
                trailLine.positionCount - 1,
                transform.position
            );
        }

        // 一定時間が経過したら弾を終了する
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
                GameObject obj = Instantiate(
                    hitEffect,
                    transform.position,
                    Quaternion.identity
                );
            }
        }

        // 敵にあたったから、レティクルを赤くする
        EnemyHP enemy = collision.collider.GetComponentInParent<EnemyHP>();

        if(enemy != null && !enemy.isDead)
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

        // 弾の当たり判定を止める
        Collider bulletCollider = GetComponent<Collider>();

        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }

        // 弾本体を見えなくする
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        // 弾道だけ残して、徐々に消す
        if (trailLine != null)
        {
            StartCoroutine(FadeTrail());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FadeTrail()
    {
        float time = 0f;

        Color color = Color.red;
        color.a = 1f;

        trailLine.startColor = color;
        trailLine.endColor = color;

        while (time < trailDuration)
        {
            time += Time.unscaledDeltaTime;

            float alpha = 1f - Mathf.Clamp01(time / trailDuration);

            color.a = alpha;

            trailLine.startColor = color;
            trailLine.endColor = color;

            yield return null;
        }

        // 弾道オブジェクトを削除
        if (trailLine != null)
        {
            Destroy(trailLine.gameObject);
        }

        // 弾本体も削除
        Destroy(gameObject);
    }
}

