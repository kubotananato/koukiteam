using UnityEngine;

public class hitEffectDestroyer : MonoBehaviour
{
    int frame = 0;
    // Update is called once per frame

    void FixedUpdate()
    {
        if (++frame > 600) Destroy(gameObject);
    }
}
