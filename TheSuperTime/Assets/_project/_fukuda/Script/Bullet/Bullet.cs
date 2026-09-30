using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 1f;

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
