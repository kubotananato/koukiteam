/*
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    public Transform weaponView;

    public GameObject heldWeapon;
    public int initialAmmo = 10;

    public float throwForce = 15f;
    public float throwSpawnDistance = 0.8f;
    public Animator PlayerThrowAnimator;

    [SerializeField] float speed = 5.0f;
    [SerializeField] float ySpeed = 0.1f;
    [SerializeField] float rotSpeed = 10.0f;

    private PlayerGun currentGun;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;

    // 手に持ったときの位置(最初に持っている銃から覚える)
    private Vector3 heldLocalPosition = Vector3.zero;
    private Quaternion heldLocalRotation = Quaternion.identity;
    private Vector3 heldLocalScale = Vector3.one;
    Input input;

    void Start()
    {
        input = GetComponent<Input>();

        playerControl = GetComponent<PlayerControl>();
        playerStatus = GetComponent<PlayerStatus>();

        // 床に置いてある銃は、拾われるまで撃てないようにする
        WeaponPickupItem[] items =
            FindObjectsByType<WeaponPickupItem>(FindObjectsSortMode.None);

        foreach (WeaponPickupItem item in items)
        {
            if (heldWeapon != null && item.gameObject == heldWeapon)
            {
                continue;
            }
            SetHeld(item.gameObject, false);
        }

        // 最初から持っている銃の設定
        if (heldWeapon != null)
        {
            heldLocalPosition = heldWeapon.transform.localPosition;
            heldLocalRotation = heldWeapon.transform.localRotation;
            heldLocalScale = heldWeapon.transform.localScale;

            // 拾えるように WeaponPickupItem を付けておく
            if (heldWeapon.GetComponent<WeaponPickupItem>() == null)
            {
                heldWeapon.AddComponent<WeaponPickupItem>();
            }

            currentGun = heldWeapon.GetComponentInChildren<PlayerGun>(true);

            if (currentGun != null)
            {
                currentGun.currentAmmo = initialAmmo;
                LinkGun(currentGun);
            }

            SetHeld(heldWeapon, true);

            // 最初から武器を持っている状態にするときのアニメーション
            if (PlayerThrowAnimator != null)
            {
                PlayerThrowAnimator.SetBool("HasWeapon", true);
            }

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

    // 床の銃を拾う(PlayerWeaponpick.cs から呼ばれる)
    public void PickupWeapon(WeaponPickupItem item)
    {
        if (item == null || weaponView == null)
        {
            return;
        }

        // すでに銃を持っているときは拾わない
        if (heldWeapon != null)
        {
            Debug.Log("すでに銃を持っています");
            return;
        }

        GameObject gun = item.gameObject;
        PlayerGun gunScript = gun.GetComponentInChildren<PlayerGun>(true);

        if (gunScript == null)
        {
            Debug.LogError("この銃に PlayerGun がありません");
            return;
        }

        // カメラの下(WeaponView)に移動して、手に持つ位置に合わせる
        gun.transform.SetParent(weaponView);
        gun.transform.localPosition = heldLocalPosition;
        gun.transform.localRotation = heldLocalRotation;
        gun.transform.localScale = heldLocalScale;

        heldWeapon = gun;
        currentGun = gunScript;

        LinkGun(currentGun);
        SetHeld(gun, true);

        if (playerWeaponpick != null)
        {
            playerWeaponpick.playerGun = currentGun;
            playerWeaponpick.hasWeapon = true;
        }

        if (PlayerThrowAnimator != null)
        {
            PlayerThrowAnimator.SetTrigger("Pickup");
            PlayerThrowAnimator.SetBool("HasWeapon", true);
        }

        Debug.Log("銃を拾った! 残り弾数:" + currentGun.currentAmmo);
    }

    // 右クリックで銃を投げる
    public void ThrowWeapon()
    {
        if (heldWeapon == null ||
            playerWeaponpick == null ||
            !playerWeaponpick.hasWeapon)
        {
            return;
        }

        GameObject gun = heldWeapon;

        Transform aim = Camera.main != null
            ? Camera.main.transform
            : transform;

        // カメラの下から外して、目の前に出す
        gun.transform.SetParent(null);
        gun.transform.position = aim.position + aim.forward * throwSpawnDistance;

        // 物理で動く状態にして、飛ばす
        SetHeld(gun, false);

        Rigidbody rb = gun.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(aim.forward * throwForce, ForceMode.Impulse);
        rb.maxAngularVelocity = Mathf.Max(rb.maxAngularVelocity, rotSpeed);
        rb.angularVelocity = Random.onUnitSphere * rotSpeed;

        // 投げた直後は時間を通常速度に戻す
        if (playerControl != null)
        {
            playerControl.OnShoot();
        }

        heldWeapon = null;
        currentGun = null;

        playerWeaponpick.playerGun = null;
        playerWeaponpick.hasWeapon = false;

        if (PlayerThrowAnimator != null)
        {
            PlayerThrowAnimator.SetTrigger("Throw");
            PlayerThrowAnimator.SetBool("HasWeapon", false);
        }

        Debug.Log("銃を投げた!");
    }

    // 銃にプレイヤーの情報を渡す
    void LinkGun(PlayerGun gun)
    {
        gun.playerWeaponpick = playerWeaponpick;
        gun.playerControl = playerControl;
        gun.playerStatus = playerStatus;
        gun.PshotAnimation = PlayerThrowAnimator;
    }

    // true = 手に持つ状態、false = 床に置く・投げる状態
    void SetHeld(GameObject gun, bool held)
    {
        // 撃つスクリプトと反動のスクリプトのオン・オフ
        foreach (PlayerGun g in gun.GetComponentsInChildren<PlayerGun>(true))
        {
            g.enabled = held;
        }

        foreach (PlayerWeaponRecoil r in
                 gun.GetComponentsInChildren<PlayerWeaponRecoil>(true))
        {
            r.enabled = held;
            if (held)
            {
                r.ResetOrigin();
            }
        }

        // 当たり判定:持っている間は外す(カメラの邪魔になるため)
        foreach (Collider c in gun.GetComponentsInChildren<Collider>(true))
        {
            c.enabled = !held;
        }

        // 物理:持っている間は止める、投げるときは動かす
        Rigidbody rb = gun.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gun.AddComponent<Rigidbody>();
        }

        if (held)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    public Animator armsAnimator;

    public GameObject heldWeapon;
    public int initialAmmo = 10;

    public GameObject worldWeaponPrefab;
    public float throwForce = 15f;
    public float throwSpawnDistance = 0.8f;
    public float throwScale = 0.5f;

    private PlayerGun currentGun;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerStatus = GetComponent<PlayerStatus>();

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

            if (armsAnimator != null)
            {
                armsAnimator.SetBool("HasWeapon", true);
            }
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
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            ThrowWeapon();
        }
    }

    // 床の銃を拾う(PlayerWeaponpick.cs から呼ばれる)
    public void PickupWeapon(WeaponPickupItem item)
    {
        if (item == null || heldWeapon == null || currentGun == null)
        {
            return;
        }

        // すでに銃を持っているときは拾わない
        if (heldWeapon.activeSelf)
        {
            Debug.Log("すでに銃を持っています");
            return;
        }

        // 床の銃の弾数を引き継ぐ
        currentGun.currentAmmo = item.ammo;
        currentGun.enabled = true;

        // 手持ちの銃を再び表示する
        heldWeapon.SetActive(true);

        if (playerWeaponpick != null)
        {
            playerWeaponpick.playerGun = currentGun;
            playerWeaponpick.hasWeapon = true;
        }

        if (armsAnimator != null)
        {
            armsAnimator.SetTrigger("Pickup");
            armsAnimator.SetBool("HasWeapon", true);
        }

        // 床の銃を消す
        Destroy(item.gameObject);

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

        // 手持ちの銃は消さずに、非表示にするだけ
        if (currentGun != null)
        {
            currentGun.enabled = false;
        }
        heldWeapon.SetActive(false);

        playerWeaponpick.playerGun = null;
        playerWeaponpick.hasWeapon = false;

        if (armsAnimator != null)
        {
            armsAnimator.SetTrigger("Throw");
            armsAnimator.SetBool("HasWeapon", false);
        }

        // 投げた直後は時間を通常速度に戻す
        if (playerControl != null)
        {
            playerControl.OnShoot();
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