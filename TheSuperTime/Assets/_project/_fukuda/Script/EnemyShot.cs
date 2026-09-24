using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]GameObject bulletPrefab;
    int frameTimer = 0;
    const int SHOT_SPAN = 50;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
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
