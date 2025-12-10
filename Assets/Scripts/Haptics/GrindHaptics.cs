using UnityEngine;
using Oculus.Haptics;

public class GrindHaptics : MonoBehaviour
{
    public HapticClip grindClip;
    public string swordTag = "Sword";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(swordTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.clip = grindClip;
            source.Play();
            Debug.Log("GRIND Haptic START for: " + other.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(swordTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            // keeps playing while grinding
            source.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(swordTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.Stop();
            Debug.Log("GRIND Haptic STOP for: " + other.name);
        }
    }
}
