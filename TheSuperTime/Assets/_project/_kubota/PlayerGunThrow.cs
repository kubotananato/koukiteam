using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    public GameObject heldWeapon;

    // 投げる強さ
    public float throwForce = 15f;
    void Start()
    {
        
    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Throw();

        }
    }

    void Throw()
    {
        
        if (!playerWeaponpick.hasWeapon)
        {
            return;
        }

        // 投げる武器が設定されていなければ何もしない
        if (heldWeapon == null)
        {
            Debug.Log("投げる武器がない");
            return;
        }

        // 武器の物理挙動を有効にする
        Rigidbody rb = heldWeapon.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("武器にRigidbodyが必要です");
            return;
        }
        // 手元から武器を切り離す
        heldWeapon.transform.SetParent(null);

        // 物理挙動を有効にする
        rb.isKinematic = false;

        // プレイヤーの向いている方向へ投げる
        rb.AddForce(
            transform.forward * throwForce,
            ForceMode.Impulse
        );

        // 武器を持っていない状態にする
        SetHeldWeapon(null);

        Debug.Log("武器を投げた！");

    }

    public void SetHeldWeapon (GameObject weapon)
    {
        if (heldWeapon == null)
        {
            PlayerGun oldGun = heldWeapon.GetComponent<PlayerGun>();
            if (oldGun != null)
            {
                oldGun.enabled = false;
            }
        }

        heldWeapon = weapon;
        playerWeaponpick.hasWeapon = (weapon != null);

        // 新しく持つ銃を有効にする
        if (heldWeapon != null)
        {
            PlayerGun newGun = heldWeapon.GetComponent<PlayerGun>();

            if (newGun != null)
                newGun.enabled = true;
        }

    }

        

}
