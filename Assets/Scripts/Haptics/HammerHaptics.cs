using UnityEngine;
using Oculus.Haptics;

public class HammerHaptics : MonoBehaviour
{
    public HapticClip hammerClip;   
    public string hammerTag = "Hammer";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(hammerTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.clip = hammerClip;
            source.Play();
            Debug.Log("HAMMER HAPTIC START for: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(hammerTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.Stop();
            Debug.Log("HAMMER HAPTIC STOP for: " + other.name);
        }
    }
}
