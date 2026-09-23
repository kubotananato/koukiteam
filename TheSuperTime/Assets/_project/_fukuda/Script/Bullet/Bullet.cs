using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * 0.1f;
    }
}
