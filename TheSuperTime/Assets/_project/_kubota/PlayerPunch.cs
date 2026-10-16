
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerWeaponpick playerWeaponpick;

    public float punchDistance = 2f;
    public int punchDamage = 1;
    public float punchCooldown = 0.5f;
    public Animator PpunchAnimation;

    private float nextPunchTime = 0f;

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

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Punch();
        }
    }

    void Punch()
    {
        if (Time.time < nextPunchTime)
        {
            return;
        }

        nextPunchTime = Time.time + punchCooldown;

        Debug.Log("パンチ！");
        if (PpunchAnimation == null) return;
        PpunchAnimation.SetTrigger("PlayerPunching");

        // カメラの正面にRaycastを飛ばす
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            punchDistance))
        {
            Debug.Log("パンチが当たった：" + hit.collider.name);

        }
    }
    public void punch()
    {

    }
}

