using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float PbulletSpeed = 10f;
    public float PbulletTime = 3.0f;
    void Start()
    {
        Destroy(gameObject, PbulletTime);
    }

    void Update()
    {
        transform.position += transform.forward * PbulletSpeed * Time.deltaTime;
    }

    void OncollisinEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
