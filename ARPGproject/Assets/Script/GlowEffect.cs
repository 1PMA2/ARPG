using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private Renderer renderer;
    private MaterialPropertyBlock propBlock;
    private static readonly int GlowIntensityID = Shader.PropertyToID("_GlowIntensity");
    private static readonly int ColorID = Shader.PropertyToID("_Color");

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void SetGlowIntensity(float intensity)
    {
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat(GlowIntensityID, intensity);
        renderer.SetPropertyBlock(propBlock);
    }

    public void SetColor(Color color)
    {
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(ColorID, color);
        renderer.SetPropertyBlock(propBlock);
    }
}
