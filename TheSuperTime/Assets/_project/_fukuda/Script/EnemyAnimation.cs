using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
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
}
