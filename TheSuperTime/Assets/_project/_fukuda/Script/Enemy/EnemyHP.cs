using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int maxHp = 1;
    
    int hp;
    public bool isDead = false;
    EnemyAnimation anim;
    EnemyConfig config;
    int frame = 0;
    bool setOnece = false;
    [SerializeField] GameObject proxy;

    void Start()
    {
        hp = maxHp;
        anim = this.GetComponent<EnemyAnimation>();
        config = this.GetComponent<EnemyConfig>();
        proxy.SetActive(true);
    }

    void Update()
    {
        if(hp <= 0)
        {
            isDead = true;
            if(!setOnece)
            {
                proxy.SetActive(false);
                anim.SetDead(true, frame % 2 == 0);
                //GameManeger.Instance.downEnemy++;
                setOnece = true;
            }
        }
    }

    void FixedUpdate()
    {
        frame++;
        
    }

    public void Damaged()
    {
        hp--;
    }
}
