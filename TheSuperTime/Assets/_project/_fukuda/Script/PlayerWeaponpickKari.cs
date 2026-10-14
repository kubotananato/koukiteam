using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponpickKari : MonoBehaviour
{
    public Camera PlayerCamera;
    public PlayerGun playerGun;

    public float pickupDistance = 3f;
    public bool hasWeapon = false;
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
        if (input.inputs["pick"])
        {
            PickupWeapon();
        }

    }

    void PickupWeapon()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(ViewportX, ViewportY, 0));

        if(Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
           // if(hit.collider.CompareTag("Weapon"))
            if(hit.collider.CompareTag("Wall"))
            {
                hasWeapon = true;
                hasWeaponyes = true;
                playerGun.ReloadAmmo();
                Debug.Log("武器を拾った!");
            }
        }

    }
}
