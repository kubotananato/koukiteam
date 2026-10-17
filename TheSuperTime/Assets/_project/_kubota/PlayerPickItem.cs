using UnityEngine;

public class WeaponPickupItem : MonoBehaviour
{
    [Header("銃の設定")]

    // 手元に表示するモデル
    public GameObject heldVisualPrefab;

    // 投げたときに出す銃
    public GameObject worldWeaponPrefab;

    // 拾ったときの弾数
    public int ammo = 10;
}