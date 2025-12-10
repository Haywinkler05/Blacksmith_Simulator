using UnityEngine;
using Oculus.Haptics;

public class ForgeHaptics : MonoBehaviour
{
    public HapticClip forgeClip;

    public string rightHandTag = "RightHand";
    public string leftHandTag = "LeftHand";

    private void OnTriggerEnter(Collider other)
    {
        if (!IsHand(other)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.clip = forgeClip;
            source.Play();
            Debug.Log("Forge Haptic START for HAND: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsHand(other)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.Stop();
            Debug.Log("Forge Haptic STOP for HAND: " + other.name);
        }
    }

    private bool IsHand(Collider col)
    {
        return col.CompareTag(rightHandTag) || col.CompareTag(leftHandTag);
    }
}
