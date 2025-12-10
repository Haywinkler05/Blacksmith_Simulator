using UnityEngine;

public class WeaponGrabber : MonoBehaviour
{
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    private Rigidbody rb;
    private bool isGrabbed = false;
    private Transform grabbedBy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        // Left controller
        if (other.CompareTag("LeftHand") && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            Grab(leftHandAnchor);
        }

        // Right controller
        if (other.CompareTag("RightHand") && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            Grab(rightHandAnchor);
        }
    }

    void Grab(Transform hand)
    {
        if (isGrabbed) return;

        isGrabbed = true;
        grabbedBy = hand;

        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (!isGrabbed) return;

        // Drop if trigger released
        if (
            (grabbedBy == leftHandAnchor && !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) ||
            (grabbedBy == rightHandAnchor && !OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
           )
        {
            Drop();
        }
    }

    void Drop()
    {
        isGrabbed = false;
        transform.SetParent(null);

        rb.isKinematic = false;
        grabbedBy = null;
    }
}
