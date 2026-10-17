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

    private float nextPunchTime = 0f;
    [SerializeField] Input input;

    void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        // 銃を持っている場合はパンチしない
        if (playerWeaponpick != null &&
            playerWeaponpick.hasWeapon)
        {
            return;
        }
        if (input.inputs["shot"])
        {
            // Rayの先に Weapon タグの物があるときは、パンチしない
            if (IsLookingAtWeapon())
            {
                return;
            }

            Punch();
        }
    }
    // 画面中央のRayの先に、Weapon タグの物があるか調べる
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
            // 当たった物か、その親に Weapon タグが付いているか調べる
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

        // カメラの正面にRaycastを飛ばして、当たったか調べる
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            punchDistance,
            ~0,
            QueryTriggerInteraction.Ignore))
        {
            if (IsEnemy(hit.collider))
            {
                Debug.Log("パンチが当たった：" + hit.collider.name);

                // レティクルを赤くする
                if (reticleHit != null)
                {
                    reticleHit.Hit();
                }

                
            }
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
