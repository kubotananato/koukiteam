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
        go.tag = "PlayerPunch";

        go.transform.position =
            playerCamera.transform.position +
            playerCamera.transform.forward * hitboxDistance;
        go.transform.rotation = playerCamera.transform.rotation;

        BoxCollider box = go.AddComponent<BoxCollider>();
        box.isTrigger = false;
        box.size = hitboxSize;

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.mass = 0.01f;
        rb.linearDamping = 100f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        foreach (Collider c in
                 playerCamera.transform.root.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(box, c);
        }

        PunchHitbox hitbox = go.AddComponent<PunchHitbox>();
        hitbox.Init(hitboxLifetime, OnPunchHit);
    }

    void OnPunchHit(EnemyHP enemy)
    {
        Debug.Log("パンチが当たった：" + enemy.name);

        ReticleHit target = reticleHit != null ? reticleHit : ReticleHit.Instance;

        if (target != null)
        {
            target.Hit();
        }
        else
        {
            Debug.LogWarning("ReticleHit が見つかりません");
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
