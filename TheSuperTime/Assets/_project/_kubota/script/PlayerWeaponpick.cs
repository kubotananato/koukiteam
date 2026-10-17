using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponpick : MonoBehaviour
{
    public Camera PlayerCamera;
    public PlayerGun playerGun;
    public PlayerThrow player;

    public float pickupDistance = 3f;

    public bool hasWeapon = true;
    public bool hasWeaponyes = false;

    private float ViewportX = 0.5f;
    private float ViewportY = 0.5f;
    Input input;
    void Start()
    {
        input = GetComponent<Input>();
    }
    void Update()
    {
        hasWeaponyes = false;

        if (Mouse.current == null)
            return;

        if (input.inputs["pick"])
        {
            PickupWeapon();
        }
    }

    void PickupWeapon()
    {
        if (PlayerCamera == null || player == null)
            return;

        Ray ray = PlayerCamera.ViewportPointToRay(
            new Vector3(ViewportX, ViewportY, 0)
        );

        if (!Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
            return;

        WeaponPickupItem item =
            hit.collider.GetComponentInParent<WeaponPickupItem>();

        if (item == null)
            return;


        // 銃を持っていないとき、または弾切れの銃を
        // 持っているときに拾う
        if (hasWeapon &&
            playerGun != null &&
            playerGun.currentAmmo > 0)
        {
            Debug.Log("まだ銃を持っているよ！");
            return;
        }

        hasWeaponyes = true;
        player.PickupWeapon(item);
    }
}