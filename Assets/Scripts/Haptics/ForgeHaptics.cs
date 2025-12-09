using UnityEngine;
using Oculus.Haptics;

public class ForgeHaptics : MonoBehaviour
{
    public HapticClip forgeClip;

    private void OnTriggerEnter(Collider other)
    {
        HapticSource source = other.GetComponent<HapticSource>();

        if (source != null)
        {
            source.clip = forgeClip;
            source.Play();
            Debug.Log("Forge Haptic START for: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HapticSource source = other.GetComponent<HapticSource>();

        if (source != null)
        {
            source.Stop();
            Debug.Log("Forge Haptic STOP for: " + other.name);
        }
    }
}
