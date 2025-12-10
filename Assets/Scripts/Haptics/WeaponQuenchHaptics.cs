using UnityEngine;
using Oculus.Haptics;

public class WeaponQuenchHaptics : MonoBehaviour
{
    public HapticClip quenchClip;
    public string weaponTag = "Sword";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(weaponTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.clip = quenchClip;
            source.Play();
            Debug.Log("Quench HAPTIC START for: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(weaponTag)) return;

        HapticSource source = other.GetComponent<HapticSource>();
        if (source != null)
        {
            source.Stop();
            Debug.Log("Quench HAPTIC STOP for: " + other.name);
        }
    }
}
