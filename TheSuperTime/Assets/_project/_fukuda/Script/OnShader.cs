using UnityEngine;

public class OnShader : MonoBehaviour
{
    [SerializeField] Material mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat.SetFloat("_Intensity", 1.0f);
        mat.SetFloat("_Curvature", 0.075f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
