
using TMPro;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    [SerializeField] TMP_Text ammo;
    [SerializeField] TMP_Text maxAmmo;
    PlayerGun gun;
    [SerializeField]GameObject gunObj;
    [SerializeField] UnityEngine.UI.Image weapon;
    [SerializeField] Sprite gunImg;
    [SerializeField] Sprite punchImg;
    PlayerWeaponpick pick;
    void Start()
    {
        pick = GameObject.Find("Player").GetComponent<PlayerWeaponpick>();
        gun = gunObj.GetComponent<PlayerGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pick.hasWeapon)
        {
            ammo.text = ""+gun.currentAmmo;
            maxAmmo.text = ""+gun.maxAmmo;  
            weapon.sprite = gunImg;
        }
        else
        {
            ammo.text = "∞";
            maxAmmo.text = "∞";
            weapon.sprite = punchImg;
        }
    }
}
