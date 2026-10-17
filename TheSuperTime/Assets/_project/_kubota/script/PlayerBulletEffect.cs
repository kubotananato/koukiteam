
using UnityEngine;
using System.Collections;

public class PlayerBulletEffect : MonoBehaviour
{
    public Material trailMaterial;

    [Header("弾道の設定")]
    public float trailWidth = 0.025f;
    public float trailDuration = 0.8f;

    // 弾が飛んだ軌跡を表示する
    public void ShowTrail(Vector3 start, Vector3 end)
    {
        GameObject trailObject = new GameObject("BulletTrail");

        LineRenderer line = trailObject.AddComponent<LineRenderer>();

        line.positionCount = 2;
        line.useWorldSpace = true;

        line.startWidth = trailWidth;
        line.endWidth = trailWidth;

        // 赤色にする
        line.startColor = Color.red;
        line.endColor = Color.red;

        // マテリアルを設定
        if (trailMaterial != null)
        {
            line.material = trailMaterial;
        }
        else
        {
            Shader shader = Shader.Find("Sprites/Default");

            if (shader != null)
            {
                line.material = new Material(shader);
            }
        }

        // 最初は始点に線を縮めておく
        line.SetPosition(0, start);
        line.SetPosition(1, start);

        StartCoroutine(AnimateTrail(line, trailObject, start, end));
    }

    private IEnumerator AnimateTrail(
        LineRenderer line,
        GameObject trailObject,
        Vector3 start,
        Vector3 end)
    {
        float elapsedTime = 0f;

        // まず線が伸びる
        float growDuration = 0.08f;

        while (elapsedTime < growDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsedTime / growDuration);
            Vector3 currentEnd = Vector3.Lerp(start, end, progress);

            line.SetPosition(0, start);
            line.SetPosition(1, currentEnd);

            yield return null;
        }

        // 線を完成させる
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        // 完成した線を徐々に透明にする
        elapsedTime = 0f;

        Color color = Color.red;
        color.a = 1f;

        line.startColor = color;
        line.endColor = color;

        while (elapsedTime < trailDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsedTime / trailDuration);

            color.a = 1f - progress;

            line.startColor = color;
            line.endColor = color;

            yield return null;
        }

        Destroy(trailObject);
    }
}

