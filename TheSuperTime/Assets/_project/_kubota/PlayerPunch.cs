using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Animator PpunchAnimation;

    public void punch()
    {
        if (PpunchAnimation == null) return;
        PpunchAnimation.SetTrigger("PlayerPunching");
    }
}
