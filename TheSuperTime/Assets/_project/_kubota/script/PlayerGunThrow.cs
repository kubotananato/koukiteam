using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    public Animator armsAnimator;

    public GameObject heldWeapon;
    public int initialAmmo = 3;

    public GameObject worldWeaponPrefab;
    public float throwForce = 15f;
    public float throwSpawnDistance = 0.8f;
    public float throwScale = 0.5f;
    public float throwSpinSpeed = 15f;

    private PlayerGun currentGun;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;
    Input input;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerStatus = GetComponent<PlayerStatus>();
        input = GetComponent<Input>();

        if (heldWeapon != null)
        {
            currentGun = heldWeapon.GetComponentInChildren<PlayerGun>(true);

            if (currentGun != null)
            {
                LinkGun(currentGun);
                currentGun.currentAmmo = initialAmmo;
                currentGun.enabled = true;
            }

            heldWeapon.SetActive(true);
        }

        if (playerWeaponpick != null)
        {
            playerWeaponpick.player = this;
            playerWeaponpick.hasWeapon = heldWeapon != null;
            playerWeaponpick.playerGun = currentGun;
        }
    }

    void Update()
    {
        if (Mouse.current != null &&
            input.inputs["throw"])
        {
            ThrowWeapon();
        }
    }

    // 床の銃を拾う
    public void PickupWeapon(GameObject weaponObject)
    {
        if (weaponObject == null || heldWeapon == null || currentGun == null)
        {
            return;
        }

        // すでに銃を持っているときは拾わない
        if (heldWeapon.activeSelf)
        {
            Debug.Log("すでに銃を持っています");
            return;
        }

        WeaponPickupItem item = weaponObject.GetComponentInParent<WeaponPickupItem>();
        currentGun.currentAmmo = item != null ? item.ammo : currentGun.maxAmmo;
        currentGun.enabled = true;

        // 手持ちの銃を再び表示する
        heldWeapon.SetActive(true);

        if (playerWeaponpick != null)
        {
            playerWeaponpick.playerGun = currentGun;
            playerWeaponpick.hasWeapon = true;
        }

        // 拾った床の銃を消す
        Destroy(weaponObject);
        // 拾った時の音
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayerPickupSE();
        }

        Debug.Log("銃を拾った! 残り弾数:" + currentGun.currentAmmo);
    }

    // 右クリックで銃を投げる
    public void ThrowWeapon()
    {
        if (heldWeapon == null ||
            !heldWeapon.activeSelf ||
            playerWeaponpick == null ||
            !playerWeaponpick.hasWeapon)
        {
            return;
        }

        if (worldWeaponPrefab == null)
        {
            Debug.LogWarning("World Weapon Prefab が設定されていません");
            return;
        }

        Transform aim = Camera.main != null
            ? Camera.main.transform
            : transform;

        // カメラの少し前に、投げる銃を新しく作る
        GameObject thrown = Instantiate(
            worldWeaponPrefab,
            aim.position + aim.forward * throwSpawnDistance,
            heldWeapon.transform.rotation
        );

        // 投げる銃を小さくする
        thrown.transform.localScale = worldWeaponPrefab.transform.localScale * throwScale;

        // 今の弾数を、投げた銃に持たせる
        WeaponPickupItem item = thrown.GetComponent<WeaponPickupItem>();
        if (item != null && currentGun != null)
        {
            item.ammo = currentGun.currentAmmo;
        }

        Rigidbody rb = thrown.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("投げる銃のPrefabのルートに Rigidbody を付けてください");
            Destroy(thrown);
            return;
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(aim.forward * throwForce, ForceMode.Impulse);
        Vector3 spinAxis = aim.right + Random.insideUnitSphere * 0.2f;
        rb.angularVelocity = spinAxis.normalized * throwSpinSpeed;

        // 手持ちの銃は消さずに、非表示にするだけ
        if (currentGun != null)
        {
            currentGun.enabled = false;
        }
        heldWeapon.SetActive(false);

        playerWeaponpick.playerGun = null;
        playerWeaponpick.hasWeapon = false;

        // 投げた直後は時間を通常速度に戻す
        if (playerControl != null)
        {
            playerControl.OnShoot();
        }
        // 投げた時の音
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayerThrowSE();
        }
        Debug.Log("銃を投げた!");
    }

    // 銃にプレイヤーの情報を渡す
    void LinkGun(PlayerGun gun)
    {
        gun.playerWeaponpick = playerWeaponpick;
        gun.playerControl = playerControl;
        gun.playerStatus = playerStatus;
        gun.PshotAnimation = armsAnimator;
    }
}