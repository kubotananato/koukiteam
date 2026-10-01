using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponpick : MonoBehaviour
{
    public Camera PlayerCamera;

    public float pickupDistance = 3f;

    private float ViewportX = 0.5f;
    private float ViewportY = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            PickupWeapon();
        }
    }

    void PickupWeapon()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(ViewportX, ViewportY, 0));

        if(Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if(hit.collider.CompareTag("Weapon"))
            {
                Debug.Log("武器を拾った!");
            }
        }

    }
}
