
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PMuzzleFlashMeshEffect : MonoBehaviour
{
    public float size = 0.25f;
    public float visibleTime = 0.06f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Coroutine hideCoroutine;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        mesh.name = "MuzzleFlashMesh";

        int points = 8;
        Vector3[] vertices = new Vector3[points * 2 + 1];
        int[] triangles = new int[points * 6];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < points * 2; i++)
        {
            float angle = i * Mathf.PI / points;
            float radius = (i % 2 == 0)
                ? size
                : size * 0.35f;

            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
        }

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

        // オブジェクトは有効なまま、表示だけ消す
        meshRenderer.enabled = false;
    }

    public void Play()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning(
                "MuzzleFlashMeshの親オブジェクトが無効です"
            );
            return;
        }

        meshRenderer.enabled = true;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterTime());
    }

    IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(visibleTime);

        meshRenderer.enabled = false;
        hideCoroutine = null;
    }
}
