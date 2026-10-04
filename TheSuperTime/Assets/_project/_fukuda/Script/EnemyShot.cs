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
            Debug.Log("shot!");
            frameTimer = 0;
            ShotOneBullet();
        }
    }

    void ShotOneBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        if(obj == null) return;
        Bullet bullet = obj.GetComponent<Bullet>();
        Vector3 genePos = transform.position;
        genePos.y += 1.5f;
        obj.transform.position = genePos;
        obj.transform.rotation = transform.rotation;
    }
}
