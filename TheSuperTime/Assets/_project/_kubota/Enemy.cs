using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float eMoveSpeed = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * eMoveSpeed * Time.deltaTime;
    }
}
