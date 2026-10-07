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
        animator.SetInteger("moveDIr", _dir);
    }
}
