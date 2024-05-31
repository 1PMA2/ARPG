using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private Renderer glowRenderer;
    private MaterialPropertyBlock propBlock;
    private static readonly int GlowIntensityID = Shader.PropertyToID("_GlowIntensity");
    private static readonly int ColorID = Shader.PropertyToID("_Color");

    void Awake()
    {
        glowRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void SetGlowIntensity(float intensity)
    {
        glowRenderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat(GlowIntensityID, intensity);
        glowRenderer.SetPropertyBlock(propBlock);
    }

    public void SetColor(Color color)
    {
        glowRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(ColorID, color);
        glowRenderer.SetPropertyBlock(propBlock);
    }
}
