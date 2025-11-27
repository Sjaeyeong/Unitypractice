using UnityEngine;

public class BackgroundScrolll : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed;

    [Header("Reference")]
    public MeshRenderer meshRenderer;
    void Start()
    {
        
    }

    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
    }
}
