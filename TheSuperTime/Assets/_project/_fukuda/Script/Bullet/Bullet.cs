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
    }
}
