using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponpick : MonoBehaviour
{
    public Camera PlayerCamera;
    public PlayerGun playerGun;

    public float pickupDistance = 3f;
    public bool hasWeapon = false;
    public bool hasWeaponyes = false;

    private float ViewportX = 0.5f;
    private float ViewportY = 0.5f;

    void Update()
    {
        hasWeaponyes = false;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            PickupWeapon();
        }
    }

    void PickupWeapon()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(
            new Vector3(ViewportX, ViewportY, 0)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            // 銃に照準が合っているか確認
            if (hit.collider.CompareTag("Weapon"))
            {
                PlayerThrow player = GetComponent<PlayerThrow>();

                if (player == null)
                {
                    player = FindFirstObjectByType<PlayerThrow>();
                }

                if (player == null)
                {
                    Debug.LogError("Playerが見つかりません");
                    return;
                }

                // 当たった銃を拾う
                player.SetHeldWeapon(hit.collider.gameObject);

                if (playerGun != null)
                {
                    playerGun.ReloadAmmo();
                }

                hasWeaponyes = true;
                Debug.Log("武器を拾った!");
            }
        }
    }
}

