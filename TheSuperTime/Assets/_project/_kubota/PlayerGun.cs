using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGun : MonoBehaviour
{
    public PlayerWeaponpick playerWeaponpick; // PlayerWeapnpickのスクリプトを参照
    public PlayerStatus playerStatus; // PlayerStatusを参照
    public GameObject PbulletPrefab;
    public Transform PbulletPoint;
    //    public Camera playerCamera;

    public float PlayerGunCoolDown = 1f;

    public int maxAmmo = 10;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (playerStatus.isDead)
        {
            return;
        }
        if (playerWeaponpick.hasWeaponyes)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 武器を持っていなかったら殴る
            if (!playerWeaponpick.hasWeapon)
            {
                Debug.Log("素手攻撃");
            }
            else
            {
                // 武器を持っているときだけ撃つ
                    Debug.Log("銃");
                    PShoot();
            }
        }

    }

    void PShoot()
    {
        if(currentAmmo <= 0)
        {
            Debug.Log("弾切れ!");
            return;
        }

        currentAmmo--;

        // 弾を発射する
        Instantiate(PbulletPrefab, PbulletPoint.position, PbulletPoint.rotation);
        Debug.Log("撃った! 残りの弾数:" + currentAmmo);

        /*
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

       RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100;
        }

        Vector3 direction = targetPoint - PbulletPoint.position;

        GameObject bullet = Instantiate(PbulletPrefab, PbulletPoint.position, Quaternion.LookRotation(direction));
        */
    }
    public void ReloadAmmo()
    {
        currentAmmo = maxAmmo;

        Debug.Log("弾数が回復した！");
    }
}
