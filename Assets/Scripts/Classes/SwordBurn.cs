using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SwordBurn : MonoBehaviour
{
    public Renderer swordRenderer;

    [Range(0, 100)]
    public int testScore = 100;

    Material goodMat;
    Material burntMat;
    MaterialPropertyBlock block;

    void Awake()
    {
        if (swordRenderer == null)
            swordRenderer = GetComponentInChildren<Renderer>();

        // Get the original assigned material  
        goodMat = swordRenderer.sharedMaterial;

        // Duplicate it to make a burnt version  
        burntMat = new Material(goodMat);

        // Customize burnt version  
        burntMat.SetFloat("_Metallic", 0f);        // very non-metal
        burntMat.SetFloat("_Smoothness", 0.05f);   // rough/burned
        burntMat.SetColor("_BaseColor", Color.black);

        // Optional: glowing heat edges  
        burntMat.EnableKeyword("_EMISSION");
        burntMat.SetColor("_EmissionColor", new Color(0.1f, 0, 0));

        block = new MaterialPropertyBlock();
    }

    [ContextMenu("Apply Burn Test")]
    public void ApplyBurnTest()
    {
        ApplyBurnFromScore(testScore);
    }

    public void ApplyBurnFromScore(int score)
    {
        float t = Mathf.Clamp01(1f - (score / 100f));
        // 0 = good sword  
        // 1 = fully burned

        // blend base color  
        Color goodColor = goodMat.GetColor("_BaseColor");
        Color burntColor = burntMat.GetColor("_BaseColor");

        Color blendedColor = Color.Lerp(goodColor, burntColor, t);

        // blend metallic  
        float metalGood = goodMat.GetFloat("_Metallic");
        float metalBurn = burntMat.GetFloat("_Metallic");

        float metallic = Mathf.Lerp(metalGood, metalBurn, t);

        // blend smoothness  
        float smoothGood = goodMat.GetFloat("_Smoothness");
        float smoothBurn = burntMat.GetFloat("_Smoothness");

        float smoothness = Mathf.Lerp(smoothGood, smoothBurn, t);

        // Apply with property block  
        swordRenderer.GetPropertyBlock(block);

        block.SetColor("_BaseColor", blendedColor);
        block.SetFloat("_Metallic", metallic);
        block.SetFloat("_Smoothness", smoothness);

        // Optional emission blending  
        Color emissionGood = goodMat.GetColor("_EmissionColor");
        Color emissionBurn = burntMat.GetColor("_EmissionColor");
        block.SetColor("_EmissionColor", Color.Lerp(emissionGood, emissionBurn, t));

        swordRenderer.SetPropertyBlock(block);
    }
}
