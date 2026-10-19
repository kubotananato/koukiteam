using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGun : MonoBehaviour
{

    public PlayerWeaponpick playerWeaponpick; // PlayerWeapnpickのスクリプトを参照
    public PlayerStatus playerStatus; // PlayerStatusを参照
    public PMuzzleFlashMeshEffect muzzleFlash; // PMuzzleFlashMeshEffectを参照
    public PlayerWeaponRecoil weaponRecoil; // PlayerWeaponRecoilを参照
    public PlayerBulletEffect PbulletEffect; // PlayerBulletEffectを参照
    public PlayerPunch Ppunch;
    public PlayerControl playerControl;
    public GameObject PbulletPrefab;
    public Transform PbulletPoint;
    public GameObject PmuzzleFlashPrefab;
    public Transform PmuzzlePoint;
    public Animator PshotAnimation;
    //    public Camera playerCamera;

    public float shootCooldown = 0.5f;   // 次に撃てるまでの秒数
    private float nextShootTime = 0f;    // 次に撃てる時刻

    public int maxAmmo = 10;
    public int currentAmmo;
    public Input input;
    int lastReplayVersion;

    void Start()
    {
        currentAmmo = maxAmmo;
        lastReplayVersion = input.replayVersion;
    }

    void Update()
    {
        
        if(lastReplayVersion != input.replayVersion)
        {
            currentAmmo = maxAmmo;
            lastReplayVersion = input.replayVersion;
        }
        

        if (playerStatus != null && playerStatus.isDead)
            return;

        if (playerWeaponpick == null)
            return;

        // 銃を持っていないと撃てない
        if (!playerWeaponpick.hasWeapon)
            return;

        // 銃を拾ったクリックでは発射しない
        if (playerWeaponpick.hasWeaponyes)
            return;

        if (Mouse.current != null &&
            input.inputs["shot"])
        {
            PShoot();
        //    SoundManager.Instance.PlayGunShot();
        }
    }

    void PShoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("弾切れ！ 右クリックで銃を投げよう");
            return;
        }

        if (Time.unscaledTime < nextShootTime)
            return;

        nextShootTime = Time.unscaledTime + shootCooldown;
        currentAmmo--;

        if (PbulletPrefab != null && PbulletPoint != null)
        {
            Instantiate(
                PbulletPrefab,
                PbulletPoint.position,
                PbulletPoint.rotation
            );
        }

        Debug.Log("撃った！ 残りの弾数：" + currentAmmo);

        if (playerControl != null)
            playerControl.OnShoot();

        if (PshotAnimation != null)
            PshotAnimation.SetTrigger("PlayerShot");

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (weaponRecoil != null)
            weaponRecoil.ShotRecoil();

        if (PmuzzleFlashPrefab != null && PmuzzlePoint != null)
        {
            GameObject effect = Instantiate(
                PmuzzleFlashPrefab,
                PmuzzlePoint.position,
                PmuzzlePoint.rotation
            );

            Destroy(effect, 0.3f);
        }
    }
    void AnimationShot()
    {
        if (PshotAnimation == null) return;
        PshotAnimation.SetTrigger("PlayerShot");
    }
    public void ReloadAmmo()
    {
        currentAmmo = maxAmmo;

        Debug.Log("弾数が回復した！");
    }
}
