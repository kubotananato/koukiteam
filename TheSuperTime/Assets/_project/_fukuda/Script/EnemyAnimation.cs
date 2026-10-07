using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
