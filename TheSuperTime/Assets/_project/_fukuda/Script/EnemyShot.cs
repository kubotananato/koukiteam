using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]GameObject bulletPrefab;
    Transform gunTransform;
    int frameTimer = 0;
    const int SHOT_SPAN = 50;

    public bool canShot = false;
    EnemyAnimation anim;

    void Awake()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
    {
        if (child.name == "HumanM_GunR")
        {
            gunTransform = child;
            break;
        }
    }
    } 

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
            //anim.SetIsShot(true);
        }
        else
        {
            //anim.SetIsShot(false);
        }
    }

    void ShotOneBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        if(obj == null) return;
        Bullet bullet = obj.GetComponent<Bullet>();
        Vector3 genePos = transform.position;
        genePos.y += 1.2f;
        obj.transform.position = gunTransform.transform.position;
        obj.transform.rotation = transform.rotation;
    }
}
