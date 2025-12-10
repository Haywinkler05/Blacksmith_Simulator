using UnityEngine;
using Oculus.Haptics;

public class HammerImpactHaptics : MonoBehaviour
{
    public HapticClip hammerImpactClip;
    public string hammerTag = "Hammer";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(hammerTag)) return;

        HapticSource source = collision.collider.GetComponent<HapticSource>();
        if (source != null)
        {
            source.clip = hammerImpactClip;
            source.Play();
            Debug.Log("Hammer IMPACT HAPTIC fired for: " + collision.collider.name);
        }
    }
}
