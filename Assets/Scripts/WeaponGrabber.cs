using UnityEngine;
using UnityEngine.XR;

public class WeaponGrabber : MonoBehaviour
{
    [Header("Controller Anchors")]
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    [Header("Hand Tags")]
    public string leftHandTag = "LeftHand";
    public string rightHandTag = "RightHand";

    [Header("Grab Offsets")]
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f); // Rotate sword upward by default

    private Rigidbody rb;
    private bool isGrabbed = false;
    private Transform grabbingHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        // LEFT HAND GRAB
        if (other.CompareTag(leftHandTag) && IsLeftTriggerPressed())
        {
            Grab(leftHandAnchor);
        }

        // RIGHT HAND GRAB
        if (other.CompareTag(rightHandTag) && IsRightTriggerPressed())
        {
            Grab(rightHandAnchor);
        }
    }

    void Update()
    {
        if (!isGrabbed) return;

        // RELEASE
        if ((grabbingHand == leftHandAnchor && !IsLeftTriggerPressed()) ||
            (grabbingHand == rightHandAnchor && !IsRightTriggerPressed()))
        {
            Drop();
        }
    }

    void Grab(Transform hand)
    {
        if (isGrabbed) return;

        isGrabbed = true;
        grabbingHand = hand;

        rb.isKinematic = true;
        transform.SetParent(hand);

        // Apply your custom alignment
        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
    }

    void Drop()
    {
        isGrabbed = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        grabbingHand = null;
    }

    // -------------------------------
    // TRIGGER INPUT FUNCTIONS
    // -------------------------------

    bool IsLeftTriggerPressed()
    {
        InputDevice left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(CommonUsages.trigger, out float value);
        return value > 0.5f;
    }

    bool IsRightTriggerPressed()
    {
        InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.trigger, out float value);
        return value > 0.5f;
    }
}
