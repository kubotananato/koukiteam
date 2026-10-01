using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float PlayerHp = 1;
    public bool isDead = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isDead)
        {
            return;
        }
/*
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵に攻撃された");
            PlayerHp -= 1;

            if (PlayerHp <= 0)
            {
                isDead = true;
            }
        }
*/
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("敵に攻撃された");
            PlayerHp -= 1;

            if (PlayerHp <= 0)
            {
                isDead = true;
                GameManeger.Instance.isPlayerDead = true;
            }
        }
    }

}
