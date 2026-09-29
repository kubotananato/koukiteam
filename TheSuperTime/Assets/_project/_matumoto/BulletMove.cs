using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float bulletSpeed = 20f;    // 弾のスピード
    public float lifeTime = 5f;        // 弾が消えるまでの時間（秒）

    void Start()
    {
        // 一定時間経ったら弾を自動消滅させる（メモリ節約のため）
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 【重要】Time.deltaTimeを掛けることで、プレイヤーが止まると弾も空中でピタッと止まります
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    // 壁や敵に当たったときの処理
    void OnCollisionEnter(Collision collision)
    {
        // 何かに当たったら弾を消す
        Destroy(gameObject);
    }
}