using UnityEngine;
using Oculus.Haptics;
using Oculus.Interaction;
using System.Linq; // IMPORTANT for FirstOrDefault()

public class HammerHaptics : MonoBehaviour
{
    [Header("Haptic Settings")]
    public HapticClip impactClip;
    public float minImpactVelocity = 0.2f;
    public float cooldown = 0.15f;

    private float lastImpactTime = 0f;

    private GrabInteractable grabInteractable;
    private HapticSource activeHand = null;

    void Awake()
    {
        grabInteractable = GetComponent<GrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("HammerHaptics requires GrabInteractable on this GameObject!");
        }
    }

    void Update()
    {
        // Get the FIRST interactor grabbing the hammer
        var interactor = grabInteractable.Interactors.FirstOrDefault();

        if (interactor != null)
        {
            activeHand = interactor.GetComponentInParent<HapticSource>();
        }
        else
        {
            activeHand = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activeHand == null) return;
        if (impactClip == null) return;

        if (Time.time - lastImpactTime < cooldown)
            return;

        float speed = collision.relativeVelocity.magnitude;
        if (speed < minImpactVelocity)
            return;

        activeHand.clip = impactClip;
        activeHand.Play();

        lastImpactTime = Time.time;
    }
}
