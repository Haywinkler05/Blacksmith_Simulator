using UnityEditor;
using UnityEngine;

public class TextureAlphaCombiner : EditorWindow
{
    Texture2D albedoTex;
    Texture2D alphaTex;

    [MenuItem("Tools/Combine Albedo + Opacity")]
    static void ShowWindow()
    {
        GetWindow<TextureAlphaCombiner>("Combine Textures");
    }

    void OnGUI()
    {
        GUILayout.Label("Combine Albedo + Opacity", EditorStyles.boldLabel);
        albedoTex = (Texture2D)EditorGUILayout.ObjectField("Albedo Texture", albedoTex, typeof(Texture2D), false);
        alphaTex = (Texture2D)EditorGUILayout.ObjectField("Opacity Texture", alphaTex, typeof(Texture2D), false);

        if (GUILayout.Button("Combine"))
        {
            if (albedoTex == null || alphaTex == null)
            {
                Debug.LogError("Please assign both textures.");
                return;
            }

            CombineTextures();
        }
    }

    void CombineTextures()
    {
        int width = albedoTex.width;
        int height = albedoTex.height;

        Texture2D output = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color[] albedoPixels = albedoTex.GetPixels();
        Color[] alphaPixels = alphaTex.GetPixels();
        Color[] outputPixels = new Color[albedoPixels.Length];

        for (int i = 0; i < outputPixels.Length; i++)
        {
            float a = alphaPixels[i].grayscale; // use grayscale for alpha
            outputPixels[i] = new Color(albedoPixels[i].r, albedoPixels[i].g, albedoPixels[i].b, a);
        }

        output.SetPixels(outputPixels);
        output.Apply();

        // Save as PNG
        byte[] bytes = output.EncodeToPNG();
        string path = EditorUtility.SaveFilePanel("Save Combined Texture", "Assets", "CombinedTexture.png", "png");

        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllBytes(path, bytes);
            Debug.Log("Saved combined texture to: " + path);
            AssetDatabase.Refresh();
        }
    }
}
