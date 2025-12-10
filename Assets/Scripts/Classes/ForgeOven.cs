using System.Collections;
using UnityEngine;

public class ForgeOven : MonoBehaviour
{
    [Header("Forge Light Sources")]
    public Light[] forgeLights;       // Drag up to 5 lights
    public float fadeSpeed = 2f;      // Light fade speed

    [Header("Audio")]
    public AudioSource fireAudio;     // Drag your loop sound source
    public float audioFadeSpeed = 1.5f;

    private bool shouldBurn;
    private bool lastState;

    void Update()
    {
        shouldBurn = GamePhase.Instance.Forge == 1;

        if (shouldBurn != lastState)
        {
            if (shouldBurn)
            {
                StartCoroutine(FadeLights(1f)); // Fade lights in
                StartCoroutine(FadeAudio(1f));  // Fade audio in
            }
            else
            {
                StartCoroutine(FadeLights(0f)); // Fade lights out
                StartCoroutine(FadeAudio(0f));  // Fade audio out
            }

            lastState = shouldBurn;
        }
    }

    private IEnumerator FadeLights(float targetIntensity)
    {
        if (forgeLights == null || forgeLights.Length == 0)
            yield break;

        float t = 0f;
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

    private IEnumerator FadeAudio(float targetVolume)
    {
        if (fireAudio == null)
            yield break;

        // Start sound if fading in
        if (targetVolume > 0f && !fireAudio.isPlaying)
            fireAudio.Play();

        float t = 0f;
        float startVolume = fireAudio.volume;

        while (t < 1f)
        {
            t += Time.deltaTime * audioFadeSpeed;

            fireAudio.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        // Stop sound if fully faded out
        if (targetVolume == 0f)
            fireAudio.Stop();
    }
}
