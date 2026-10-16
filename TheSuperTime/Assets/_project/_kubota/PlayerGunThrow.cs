/*using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    [Header("参照")]
    public PlayerWeaponpick playerWeaponpick;
    public Transform weaponView;

    [Header("最初に持つ銃")]
    public GameObject heldWeapon;
    public GameObject initialWorldWeaponPrefab;
    public int initialAmmo = 10;

    [Header("投げる設定")]
    public float throwForce = 15f;

    private GameObject currentWorldWeaponPrefab;
    private PlayerGun currentGun;
    private Vector3 heldLocalPosition;
    private Quaternion heldLocalRotation;
    private Vector3 heldLocalScale;

    void Start()
    {
        Debug.Log("PlayerThrowのStartが実行された");
        // 最初から銃を持ってスタート
        if (heldWeapon != null)
        {
            heldWeapon.SetActive(true);
            heldLocalPosition = heldWeapon.transform.localPosition;
            heldLocalRotation = heldWeapon.transform.localRotation;
            heldLocalScale = heldWeapon.transform.localScale;
            currentGun = heldWeapon.GetComponentInChildren<PlayerGun>(true);

            if (currentGun != null)
            {
                currentGun.currentAmmo = initialAmmo;
                currentGun.enabled = true;
            }
        }

        currentWorldWeaponPrefab = initialWorldWeaponPrefab;

        if (playerWeaponpick != null)
        {
            playerWeaponpick.player = this;
            playerWeaponpick.hasWeapon = heldWeapon != null;
            playerWeaponpick.playerGun = currentGun;
        }
    }

    void Update()
    {
        Debug.Log("PlayerThrowのUpdateが動いている");

        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("右クリックを検知！");
            ThrowWeapon();
        }
    }
    void OnDisable()
    {
        Debug.Log("PlayerThrow が無効になった");
    }

    void OnDestroy()
    {
        Debug.Log("PlayerThrow が破棄された");
    }

    // 床の銃を拾う
    public void PickupWeapon(WeaponPickupItem item)
    {
        if (item == null || weaponView == null)
            return;

        // 前の手元の銃を非表示にする
        if (heldWeapon != null)
        {
            heldWeapon.SetActive(false);
        }

        // 別モデルが登録されていれば、そのモデルを手元に生成
        if (item.heldVisualPrefab != null)
        {
            heldWeapon = Instantiate(item.heldVisualPrefab, weaponView);

            heldWeapon.transform.localPosition = heldLocalPosition;
            heldWeapon.transform.localRotation = heldLocalRotation;
            heldWeapon.transform.localScale = heldLocalScale;

            heldWeapon.SetActive(true);
        }
        else
        {
            Debug.LogError("heldVisualPrefabが設定されていません");
            return;
        }

        currentGun = heldWeapon.GetComponentInChildren<PlayerGun>(true);

        if (currentGun == null)
        {
            Debug.LogError("手持ち銃のPrefabにPlayerGunがありません");
            heldWeapon.SetActive(false);
            return;
        }

        currentGun.playerWeaponpick = playerWeaponpick;
        currentGun.enabled = true;
        currentGun.currentAmmo = item.ammo;
        currentWorldWeaponPrefab = item.worldWeaponPrefab;

        Debug.Log("投げるPrefab：" +
            (currentWorldWeaponPrefab != null
                ? currentWorldWeaponPrefab.name
                : "設定されていません"));

        if (playerWeaponpick != null)
        {
            playerWeaponpick.playerGun = currentGun;
            playerWeaponpick.hasWeapon = true;
        }

        // 拾った床の銃を消す
        Destroy(item.gameObject);

        Debug.Log("銃を拾った！ 残り弾数：" + currentGun.currentAmmo);
    }

    // 右クリックで銃を投げる

    public void ThrowWeapon()
    {
        Debug.Log("ThrowWeaponが呼ばれた！");

        if (heldWeapon == null ||
            !heldWeapon.activeSelf ||
            playerWeaponpick == null ||
            !playerWeaponpick.hasWeapon)
        {
            return;
        }

        if (currentWorldWeaponPrefab == null)
        {
            Debug.LogWarning("投げる銃のPrefabが設定されていません");
            return;
        }

        // 投げる銃を生成する
        Vector3 spawnPosition = heldWeapon.transform.position;
        Quaternion spawnRotation = heldWeapon.transform.rotation;

        GameObject thrownWeapon = Instantiate(
            currentWorldWeaponPrefab,
            spawnPosition,
            spawnRotation
        );

        // Rigidbodyは、できればPrefabのルートに付ける
        Rigidbody rb = thrownWeapon.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError(
                "投げる銃のPrefabのルートにRigidbodyを追加してください"
            );

            Destroy(thrownWeapon);
            return;
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // カメラが向いている方向へ投げる
        Transform aimTransform = Camera.main != null
            ? Camera.main.transform
            : transform;

        rb.AddForce(
            aimTransform.forward * throwForce,
            ForceMode.Impulse
        );

        // 手元の銃をしまう
        heldWeapon.SetActive(false);

        if (currentGun != null)
        {
            currentGun.enabled = false;
        }

        currentGun = null;
        playerWeaponpick.playerGun = null;
        playerWeaponpick.hasWeapon = false;
        currentWorldWeaponPrefab = null;

        Debug.Log("銃を投げた！");
    }

}
*/
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

    void Start()
    {
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

        Debug.Log("銃を投げた!");
    }

    // 銃にプレイヤーの情報を渡す
    void LinkGun(PlayerGun gun)
    {
        gun.playerWeaponpick = playerWeaponpick;
        gun.playerControl = playerControl;
        gun.playerStatus = playerStatus;
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