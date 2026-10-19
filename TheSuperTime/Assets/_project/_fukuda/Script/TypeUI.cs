
using UnityEngine;


public class TypeUI : MonoBehaviour
{
    [SerializeField]Camera playerCamera;
    [SerializeField] Sprite gun;
    [SerializeField] Sprite throwing;
    [SerializeField] Sprite punch;
    UnityEngine.UI.Image img;
    [SerializeField] GameObject typeObj;
    bool isLooking = false;
    EneType current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = typeObj.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        isLooking = false;
        if(playerCamera == null) return;
        Ray ray = new Ray (
            playerCamera.transform.position,
            playerCamera.transform.forward
        );
        if(Physics.Raycast(ray,out RaycastHit hit, 10000))
        {
            GameObject ene = hit.collider.gameObject;
                if(ene.CompareTag("Enemy"))
                {
                    isLooking = true;
                    EnemyConfig c = ene.GetComponent<EnemyConfig>();
                    current = c.damageType;
                }
        }

        if(!isLooking)
        {
            typeObj.SetActive(false);
            return;
        }
        typeObj.SetActive(true);
        img.sprite = gun;
        if(current == EneType.Punch) img.sprite = punch;
        if(current == EneType.Throw) img.sprite = throwing;
    }    
}
