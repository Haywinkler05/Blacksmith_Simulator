using UnityEngine;
using Oculus.Haptics;

public class ForgeHaptics : MonoBehaviour
{
    public HapticClip forgeClip; // assign your .haptic file in inspector

    private void OnTriggerEnter(Collider other)
    {
        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            // assign clip before play
            source.clip = forgeClip;
            source.Play();
        }
    }
}
