using UnityEditor.Callbacks;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    GameObject player;
    Vector3 dropDirXZ;
    Rigidbody rigid;
    [SerializeField]float speed = 5.0f;
    [SerializeField] float ySpeed = 0.1f;
    [SerializeField] float rotSpeed = 10.0f;
    ReplayTimer repT;
    void Start()
    {
        repT = GameObject.Find("ReplayManager").GetComponent<ReplayTimer>();
        player = GameObject.Find("Player");
        dropDirXZ = player.transform.position - transform.position;
        dropDirXZ.y = 0.0f;
        dropDirXZ.Normalize();
        rigid = GetComponent<Rigidbody>();
        rigid.linearVelocity = dropDirXZ * speed + Vector3.up * ySpeed; 

        // 回転させる
        rigid.maxAngularVelocity = Mathf.Max(rigid.maxAngularVelocity, rotSpeed);
        rigid.angularVelocity = Random.onUnitSphere * rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(repT.IsStartReplay())
        {
            Destroy(gameObject);
        }
    }

}
