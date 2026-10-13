using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    public float attackCoolDown = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        if (playerWeaponpick.hasWeapon == false && playerWeaponpick.hasWeaponyes == false)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("殴り");
                Attack();
            }
        }
    }

    void Attack()
    {

    }
}
