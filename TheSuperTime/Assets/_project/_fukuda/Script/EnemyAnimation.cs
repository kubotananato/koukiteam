using UnityEngine;


public class EnemyAnimation : MonoBehaviour
{
    EnemyConfig config;
    Animator animator;
    void Start()
    {
        config = GetComponent<EnemyConfig>();
        animator = GetComponent<Animator>();
        if(!config.isGun)
        {
            animator.SetBool("isPunch", true);
        }
    }

    public void SetDead(bool _isDead, bool _isForward)
    {
        animator.SetBool("isDead", _isDead);
        animator.SetBool("isForward", _isForward);
    }

    public void SetMoveDir(int _dir)
    {
        animator.SetInteger("moveDir", _dir);
    }

    public void SetIsShot(bool _isShot)
    {
        animator.SetBool("isShot", _isShot);
    }

    public void SetIsPunch(bool _isAttack)
    {
        animator.SetBool("isAttack", _isAttack);
    }
}
