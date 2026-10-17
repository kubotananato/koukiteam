using UnityEngine;
using UnityEngine.UI;

public class ReticleHit : MonoBehaviour
{
    public static ReticleHit Instance; // そのスクリプトからでもこのスクリプトを呼べるようにする

    public Image[] reticleImages;
    public Color normalColor = Color.white;
    public Color hitColor = Color.red;
    public float hitDuration = 0.15f;

    private float hitTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (hitTimer > 0f)
        {
            // スローの影響を受けないように unscaledDeltaTime を使う
            hitTimer -= Time.unscaledDeltaTime;
            SetColor(hitColor);
        }
        else
        {
            SetColor(normalColor);
        }
    }

    void SetColor(Color color)
    {
        foreach (Image img in reticleImages)
        {
            if (img != null)
            {
                img.color = color;
            }
        }
    }

    // 攻撃が当たったときに呼ぶ
    public void Hit()
    {
        Debug.Log("ReticleHit.Hit が呼ばれた / Imageの数 = " + reticleImages.Length);
        hitTimer = hitDuration;
    }
}