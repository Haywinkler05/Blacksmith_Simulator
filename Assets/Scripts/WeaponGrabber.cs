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
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

    private Rigidbody rb;
    private bool isGrabbed = false;
    private Transform grabbingHand;

    // --- Velocity Tracking ---
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 trackedVelocity;
    private Vector3 trackedAngularVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGrabbed)
        {
            if (other.CompareTag(leftHandTag) && IsLeftTriggerPressed())
            {
                Grab(leftHandAnchor);
            }
            else if (other.CompareTag(rightHandTag) && IsRightTriggerPressed())
            {
                Grab(rightHandAnchor);
            }
        }
    }

    void Update()
    {
        if (!isGrabbed) return;

        // RELEASE LOGIC
        if ((grabbingHand == leftHandAnchor && !IsLeftTriggerPressed()) ||
            (grabbingHand == rightHandAnchor && !IsRightTriggerPressed()))
        {
            Drop();
        }
    }

    void LateUpdate()
    {
        if (isGrabbed)
        {
            TrackVelocity();
        }
    }

    // -------------------------------
    // GRAB
    // -------------------------------
    void Grab(Transform hand)
    {
        if (isGrabbed) return;

        isGrabbed = true;
        grabbingHand = hand;

        rb.isKinematic = true;

        transform.SetParent(hand);
        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;

        // Initialize tracking baseline
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        trackedVelocity = Vector3.zero;
        trackedAngularVelocity = Vector3.zero;
    }

    // -------------------------------
    // DROP
    // -------------------------------
    void Drop()
    {
        isGrabbed = false;
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.velocity = trackedVelocity;
        rb.angularVelocity = trackedAngularVelocity;

        grabbingHand = null;
    }

    // -------------------------------
    // VELOCITY TRACKING
    // -------------------------------
    void TrackVelocity()
    {
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        trackedVelocity = (currentPos - lastPosition) / Time.deltaTime;

        Quaternion delta = currentRot * Quaternion.Inverse(lastRotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;

        trackedAngularVelocity = axis * (angle * Mathf.Deg2Rad / Time.deltaTime);

        lastPosition = currentPos;
        lastRotation = currentRot;
    }

    // -------------------------------
    // TRIGGER INPUT
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

    // -------------------------------
    // Public getter for swing velocity
    // -------------------------------
    public float GetSwingVelocity()
    {
        return trackedVelocity.magnitude;
    }
}
