using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]GameObject bulletPrefab;
    int frameTimer = 0;
    const int SHOT_SPAN = 50;

    public bool canShot = false;

    void Start()
    {
        frameTimer = 30;
    }

    void FixedUpdate()
    {
        // とりあえず仮で目標地点まで到達→撃つの流れ
        if (!canShot) return;

        frameTimer++;
        if(frameTimer >= SHOT_SPAN)
        {
            frameTimer = 0;
            ShotOneBullet();
        }
    }

    void ShotOneBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        if(obj == null) return;
        Bullet bullet = obj.GetComponent<Bullet>();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
}
