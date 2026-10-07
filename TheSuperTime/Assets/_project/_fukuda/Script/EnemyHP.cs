using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int maxHp = 1;
    int hp;
    public bool isDead = false;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Damaged();
        }
    }

    public void Damaged()
    {
        hp--;
        if(hp <= 0)
        {
            isDead = true;
            if (GameManeger.Instance == null) return;
            GameManeger.Instance.downEnemy++;
        }
    }
}
