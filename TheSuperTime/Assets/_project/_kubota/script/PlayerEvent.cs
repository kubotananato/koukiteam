using UnityEngine;

public class PlayerEvwent : MonoBehaviour
{
    // 子の Rig などにある PlayerPunch を探す
    private PlayerPunch playerPunch;

    void Awake()
    {
        playerPunch = GetComponentInChildren<PlayerPunch>(true);
    }

    // アニメーションのイベントから呼ばれる
    public void ActivePunchCol()
    {
        if (playerPunch != null)
        {
            playerPunch.ActivePunchCol();
        }
    }

    public void DeactivePunchCol()
    {
        if (playerPunch != null)
        {
            playerPunch.DeactivePunchCol();
        }
    }
}
