using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGun : MonoBehaviour
{
    public GameObject PbulletPrefab;
    public Transform PbulletPoint;
//    public Camera playerCamera;

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("左クリック");
           PShoot();
        }
    }

    void PShoot()
    {
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
        Debug.Log("撃った");
        Instantiate(PbulletPrefab, PbulletPoint.position, PbulletPoint.rotation);

    }
}
