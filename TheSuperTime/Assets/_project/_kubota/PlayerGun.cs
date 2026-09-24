using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGun : MonoBehaviour
{
    public GameObject PbulletPrefab;
    public Transform PbulletPoint;

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
           PShoot();
        }
    }

    void PShoot()
    {
        Instantiate(PbulletPrefab, PbulletPoint.position, PbulletPoint.rotation);
    }
}
