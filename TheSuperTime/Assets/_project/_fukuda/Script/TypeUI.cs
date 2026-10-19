
using UnityEngine;


public class TypeUI : MonoBehaviour
{
    [SerializeField]Camera playerCamera;
    [SerializeField] Sprite gun;
    [SerializeField] Sprite throwing;
    [SerializeField] Sprite punch;
    UnityEngine.UI.Image img;
    [SerializeField] GameObject typeObj;
    [SerializeField] GameObject weaponObj;
    bool isLooking = false;
    EneType current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = typeObj.GetComponent<UnityEngine.UI.Image>();
        typeObj.SetActive(false);
        weaponObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isLooking = false;
        bool isLookingWeapon = false;
        if(playerCamera == null) return;
        Ray ray = new Ray (
            playerCamera.transform.position,
            playerCamera.transform.forward
        );
        if(Physics.Raycast(ray,out RaycastHit hit, 10000))
        {
            GameObject obj = hit.collider.gameObject;
                if(obj.CompareTag("Enemy"))
                {
                    isLooking = true;
                    EnemyConfig c = obj.GetComponent<EnemyConfig>();
                    current = c.damageType;
                }
            else if(obj.CompareTag("PlayerThrow"))
            {
                GameObject player = GameObject.Find("Player");
                float dis = (player.transform.position - obj.transform.position).magnitude;
                if(dis <= 3.0f)isLookingWeapon = true;
            }
        }

        weaponObj.SetActive(isLookingWeapon);

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
