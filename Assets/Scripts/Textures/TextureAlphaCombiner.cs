using UnityEditor;
using UnityEngine;

public class TextureAlphaCombiner : EditorWindow
{
    Texture2D albedoTex;
    Texture2D alphaTex;

    [MenuItem("Tools/Combine Albedo + Opacity (Force Readable)")]
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

    Texture2D MakeReadable(Texture2D source)
    {
        // Creates a readable copy via RenderTexture → Texture2D
        RenderTexture rt = RenderTexture.GetTemporary(
            source.width, source.height, 0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readable.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readable.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readable;
    }

    void CombineTextures()
    {
        // Force readable copies
        Texture2D readableAlbedo = MakeReadable(albedoTex);
        Texture2D readableAlpha = MakeReadable(alphaTex);

        int width = readableAlbedo.width;
        int height = readableAlbedo.height;

        Texture2D output = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color[] albedoPixels = readableAlbedo.GetPixels();
        Color[] alphaPixels = readableAlpha.GetPixels();
        Color[] outputPixels = new Color[albedoPixels.Length];

        for (int i = 0; i < outputPixels.Length; i++)
        {
            float a = alphaPixels[i].grayscale;
            outputPixels[i] = new Color(albedoPixels[i].r, albedoPixels[i].g, albedoPixels[i].b, a);
        }

        output.SetPixels(outputPixels);
        output.Apply();

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
