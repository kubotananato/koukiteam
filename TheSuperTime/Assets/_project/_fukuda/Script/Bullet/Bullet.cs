using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject hitEffectPrefab;

    void Start()
    {
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        Vector3 currentPos = transform.position;
        if (currentPos.y > 30.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject obj = Instantiate(hitEffectPrefab);
            obj.transform.position = transform.position;
            Debug.Log("hit!!");
            Destroy(gameObject);
            return;
        }

        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
    }
}
