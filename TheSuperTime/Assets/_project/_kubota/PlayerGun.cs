using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGun : MonoBehaviour
{

    public PlayerWeaponpick playerWeaponpick; // PlayerWeapnpickのスクリプトを参照
    public PlayerStatus playerStatus; // PlayerStatusを参照
    public PMuzzleFlashMeshEffect muzzleFlash; // PMuzzleFlashMeshEffectを参照
    public PlayerWeaponRecoil weaponRecoil; // PlayerWeaponRecoilを参照
    public PlayerControl playerControl;
    public GameObject PbulletPrefab;
    public Transform PbulletPoint;
    public GameObject PmuzzleFlashPrefab;
    public Transform PmuzzlePoint;
    //    public Camera playerCamera;

    public float shootCooldown = 0.5f;   // 次に撃てるまでの秒数
    private float nextShootTime = 0f;    // 次に撃てる時刻

    public int maxAmmo = 10;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (playerStatus.isDead)
        {
            return;
        }
        if (playerWeaponpick.hasWeaponyes)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 武器を持っていなかったら殴る
            if (!playerWeaponpick.hasWeapon)
            {
                Debug.Log("素手攻撃");
            }
            else
            {
                // 武器を持っているときだけ撃つ
                    Debug.Log("銃");
                    PShoot();
            }
        }

    }

    void PShoot()
    {
        // クールタイム中なら撃たない(いちばん最初に確認する)
        if (Time.unscaledTime < nextShootTime) return;

        if (currentAmmo <= 0)
        {
            Debug.Log("弾切れ!");
            return;
        }

        // ここまで来たら撃てるので、次に撃てる時刻を決める
        nextShootTime = Time.unscaledTime + shootCooldown;

        currentAmmo--;

        // 弾を発射する
        Instantiate(PbulletPrefab, PbulletPoint.position, PbulletPoint.rotation);
        Debug.Log("撃った! 残りの弾数:" + currentAmmo);

        // 撃った瞬間に時間を通常速度に戻す
        if (playerControl != null)
        {
            playerControl.OnShoot();
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        if (weaponRecoil != null)
        {
            weaponRecoil.ShotRecoil();
        }
        if (PmuzzleFlashPrefab != null && PmuzzlePoint != null)
        {
            GameObject effect = Instantiate(
                PmuzzleFlashPrefab,
                PmuzzlePoint.position,
                PmuzzlePoint.rotation
            );

            // 0.3秒後にエフェクトを削除する
            Destroy(effect, 0.3f);
        }
    }
    public void ReloadAmmo()
    {
        currentAmmo = maxAmmo;

        Debug.Log("弾数が回復した！");
    }
}
