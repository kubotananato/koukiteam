using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]GameObject bulletPrefab;
    [SerializeField]GameObject gunObj;
    [SerializeField]GameObject punchColPrefab;
    GameObject tempPunch;
    int frameTimer = 0;
    const int SHOT_SPAN = 50;
    const int PUNCH_SPAN = 50;

    public bool canShot = false;
    EnemyAnimation anim;
    EnemyConfig config;

    void Awake()
    {
        
    } 

    void Start()
    {
        frameTimer = 30;
        config = this.GetComponent<EnemyConfig>();
        anim = this.GetComponent<EnemyAnimation>();
    }

    void FixedUpdate()
    {
        // とりあえず仮で目標地点まで到達→撃つの流れ
        if (!canShot) return;
        frameTimer++;
        switch(config.isGun)
        {
            case true:
            // 銃を持っているときの攻撃
            if(frameTimer >= SHOT_SPAN)
            {
                Debug.Log("shot!");
                frameTimer = 0;
                ShotOneBullet();
                anim.SetIsShot(true);
            }
            else
            {
                anim.SetIsShot(false);
            }      
            break;
            case false:
            if(tempPunch != null)
                {
                    tempPunch.transform.position = transform.position + transform.forward * 0.8f;
                }
            // 銃を持っていないときの攻撃
                if(frameTimer >= PUNCH_SPAN)
                {
                    Debug.Log("punch!");
                    frameTimer = 0;
                    anim.SetIsPunch(true);
                }
                else
                {
                    anim.SetIsPunch(false);
                }

            break;
        }

        
        
    }

    void ShotOneBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        if(obj == null) return;
        Bullet bullet = obj.GetComponent<Bullet>();
        Vector3 genePos = transform.position;
        genePos.y += 1.2f;
        obj.transform.position = gunObj.transform.position;
        obj.transform.rotation = transform.rotation;
    }

    public void ActivePunchCol()
    {
        tempPunch = Instantiate(punchColPrefab);
        tempPunch.transform.position = transform.position + transform.forward * 0.8f;
    }

    public void DeActivePunchCol()
    {
        Destroy(tempPunch);
    }
}
