using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int maxHp = 1;
    int hp;
    public bool isDead = false;
    EnemyAnimation anim;
    int frame = 0;
    bool setOnece = false;
    GameObject proxy;

    void Start()
    {
        hp = maxHp;
        anim = this.GetComponent<EnemyAnimation>();

        foreach (GameObject child in GetComponentsInChildren<GameObject>(true))
        {
            if (child.name == "HumanM_GunR")
            {
                proxy = child;
                break;
            }
        }
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
                GameManeger.Instance.downEnemy++;
                setOnece = true;
            }
        }
    }

    void FixedUpdate()
    {
        frame++;
        
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
        
    }
}
