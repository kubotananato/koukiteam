using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerWeaponpick playerWeaponpick;
    public ReticleHit reticleHit;

    public float punchDistance = 2f;
    public float punchCooldown = 0.5f;
    public Animator PpunchAnimation;
    public string enemyTag = "Enemy";

    public PlayerControl playerControl;
    public Vector3 hitboxSize = new Vector3(1f, 1f, 1.5f);
    public float hitboxDistance = 1f;
    public float hitboxLifetime = 0.2f;

    private float nextPunchTime = 0f;
    [SerializeField] Input input;

    void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

       
        if (playerWeaponpick != null &&
            playerWeaponpick.hasWeapon)
        {
            return;
        }
        if (input.inputs["shot"])
        {
            if (IsLookingAtWeapon())
            {
                return;
            }

            Punch();
        }
    }

    bool IsLookingAtWeapon()
    {
        if (playerCamera == null)
        {
            return false;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, punchDistance))
        {
            Transform t = hit.collider.transform;

            while (t != null)
            {
                if (t.CompareTag("Weapon"))
                {
                    return true;
                }
                t = t.parent;
            }
        }

        return false;
    }

    void Punch()
    {
        // スロー中でもクールタイムが正しく進むように unscaledTime を使う
        if (Time.unscaledTime < nextPunchTime)
        {
            return;
        }

        nextPunchTime = Time.unscaledTime + punchCooldown;

        Debug.Log("パンチ！");

        if (PpunchAnimation != null)
        {
            PpunchAnimation.SetTrigger("PlayerPunch");
        }

        if (playerControl != null)
        {
            playerControl.OnShoot();
        }

        SpawnHitbox();
    }

    // カメラの前に、見えない当たり判定の箱を生み出す
    void SpawnHitbox()
    {
        GameObject go = new GameObject("PunchHitbox");

        go.transform.SetParent(playerCamera.transform);
        go.transform.localPosition = new Vector3(0f, 0f, hitboxDistance);
        go.transform.localRotation = Quaternion.identity;

        // 当たり判定
        BoxCollider box = go.AddComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = hitboxSize;

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // 敵に当たったときの処理を渡す
        PunchHitbox hitbox = go.AddComponent<PunchHitbox>();
        hitbox.Init(hitboxLifetime, OnPunchHit);
    }

    void OnPunchHit(EnemyHP enemy)
    {
        Debug.Log("パンチが当たった：" + enemy.name);

        if (reticleHit != null)
        {
            reticleHit.Hit();
        }
    }

    bool IsEnemy(Collider col)
    {
        Transform t = col.transform;

        while (t != null)
        {
            if (t.CompareTag(enemyTag))
            {
                return true;
            }
            t = t.parent;
        }

        return false;
    }

    public void punch()
    {

    }

    public void ActivePunchCol()
    {
        Debug.Log("パンチの判定タイミング");
    }

    public void DeactivePunchCol()
    {
    }
}
