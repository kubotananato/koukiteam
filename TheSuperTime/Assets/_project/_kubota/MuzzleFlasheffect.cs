
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PMuzzleFlashMeshEffect : MonoBehaviour
{
    public float size = 0.25f;
    public float visibleTime = 0.06f;

    private MeshFilter meshFilter;
    private Coroutine hideCoroutine;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        // フラッシュの形を作る
        Mesh mesh = new Mesh();
        mesh.name = "MuzzleFlashMesh";

        int points = 8;
        Vector3[] vertices = new Vector3[points * 2 + 1];
        int[] triangles = new int[points * 6];

        // 中心点
        vertices[0] = Vector3.zero;

        // 外側と内側の点を交互に配置する
        for (int i = 0; i < points * 2; i++)
        {
            float angle =
                i * Mathf.PI / points;

            float radius =
                (i % 2 == 0) ? size : size * 0.35f;

            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
        }

        // 三角形をつなげる
        for (int i = 0; i < points; i++)
        {
            int index = i * 2;

            triangles[i * 6] = 0;
            triangles[i * 6 + 1] = index + 1;
            triangles[i * 6 + 2] = index + 2;

            triangles[i * 6 + 3] = 0;
            triangles[i * 6 + 4] = index + 2;
            triangles[i * 6 + 5] =
                (index + 3 > points * 2)
                ? 1
                : index + 3;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // 最初は非表示
        gameObject.SetActive(false);
    }

    public void Play()
    {
        Debug.Log("マズルフラッシュ再生！");
        gameObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterTime());
    }

    IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(visibleTime);

        gameObject.SetActive(false);
    }
}
