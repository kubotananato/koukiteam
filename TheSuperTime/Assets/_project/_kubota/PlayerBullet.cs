using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float PbulletSpeed = 10f;
    public float PbulletTime = 3.0f;
    [SerializeField] GameObject hitEffect;
    void Start()
    {
        Destroy(gameObject, PbulletTime);
    }

    void Update()
    {
        transform.position += transform.forward * PbulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameObject obj = Instantiate(hitEffect);
            obj.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}
