using UnityEngine;

public class EffectGene : MonoBehaviour
{
    [SerializeField] GameObject hitEffectPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject obj = Instantiate(hitEffectPrefab);
            obj.transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
    }
}
