using UnityEngine;

public class PlayerWeaponRecoil : MonoBehaviour
{
    public float recoilBack = -1f;
    public float recoilUp =10f;
    public float returnSpeed = 3f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 currentPosition;
    private Vector3 currentRotation;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    void Update()
    {
        currentPosition = Vector3.Lerp(currentPosition, Vector3.zero,returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero,returnSpeed * Time.deltaTime);

        // 元の位置に反動を加える
     //   transform.localPosition += recoilPositon * Time.deltaTime;
        transform.localPosition = startPosition + currentPosition;
        transform.localRotation = startRotation * Quaternion.Euler(currentRotation);

    }

    public void ShotRecoil()
    {
        // 銃を後ろに下げる
        currentPosition.z = recoilBack;
        // 銃口を上に向ける
        currentRotation.x = -recoilUp;
    }
}
