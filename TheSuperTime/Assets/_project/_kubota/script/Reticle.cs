using UnityEngine;
using UnityEngine.UI;

public class ReticleHit : MonoBehaviour
{
    public Image[] reticleImages;            // レティクルの Image(4つ全部)
    public Color normalColor = Color.white;  // 通常の色
    public Color hitColor = Color.red;       // 当たったときの色
    public float hitDuration = 0.15f;        // 赤くしている時間(秒)

    private float hitTimer = 0f;

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

    // 4つの Image をまとめて同じ色にする
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
        hitTimer = hitDuration;
    }
}