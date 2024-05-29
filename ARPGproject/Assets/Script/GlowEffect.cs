using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private Renderer renderer;
    private MaterialPropertyBlock propBlock;
    private static readonly int GlowIntensityID = Shader.PropertyToID("_GlowIntensity");

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
}
