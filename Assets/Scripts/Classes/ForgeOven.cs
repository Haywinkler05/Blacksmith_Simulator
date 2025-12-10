using System.Collections;
using UnityEngine;

public class ForgeOven : MonoBehaviour
{
    [Header("Forge Light Sources")]
    public Light[] forgeLights;     // Drag and drop up to 5 lights here
    public float fadeSpeed = 2f;    // How fast lights fade in/out

    private bool shouldBurn;
    private bool lastState;

    void Update()
    {
        // Check phase
        shouldBurn = GamePhase.Instance.Forge == 1;

        // Only react if the value changed
        if (shouldBurn != lastState)
        {
            if (shouldBurn)
                StartCoroutine(FadeLights(1f));  // Fade in
            else
                StartCoroutine(FadeLights(0f));  // Fade out

            lastState = shouldBurn;
        }
    }

    private IEnumerator FadeLights(float targetIntensity)
    {
        if (forgeLights == null || forgeLights.Length == 0)
            yield break;

        float t = 0f;

        // Cache starting intensities
        float[] startValues = new float[forgeLights.Length];
        for (int i = 0; i < forgeLights.Length; i++)
            startValues[i] = forgeLights[i].intensity;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            float lerp = Mathf.Lerp(0f, 1f, t);

            for (int i = 0; i < forgeLights.Length; i++)
            {
                if (forgeLights[i] != null)
                    forgeLights[i].intensity = Mathf.Lerp(startValues[i], targetIntensity, lerp);
            }

            yield return null;
        }
    }
}
