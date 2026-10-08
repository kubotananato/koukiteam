using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Throw()
    {
        playerWeaponpick.hasWeaponyes = false;
    }
}
