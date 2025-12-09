using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngot : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The list of prefabs available to spawn.")]
    public GameObject[] ingotPrefabs;

    [Tooltip("The index of the prefab currently selected to spawn.")]
    private int currentPrefabIndex = 0;

    [Tooltip("The tag used to identify the player's hand.")]
    public string handTag = "PlayerHand";

    [Tooltip("The input button name defined in the Input Manager for grabbing (e.g. Fire1, Grab).")]
    public string grabButtonName = "Fire1";

    [Tooltip("Key for testing in editor without VR.")]
    public KeyCode debugKey = KeyCode.G;

    private GameObject currentHand;
    private bool isHandInside = false;

    // Update is called once per frame
    void Update()
    {
        if (isHandInside && currentHand != null)
        {
            bool grabPressed = false;

            // OVR Input (Meta Quest)
            // Determine which controller corresponds to the hand
            OVRInput.Controller controller = OVRInput.Controller.None;
            if (currentHand.name.Contains("Left") || currentHand.name.Contains("LHand"))
            {
                controller = OVRInput.Controller.LTouch;
            }
            else if (currentHand.name.Contains("Right") || currentHand.name.Contains("RHand"))
            {
                controller = OVRInput.Controller.RTouch;
            }

            // Check the specific controller if found, otherwise check both (or active)
            if (controller != OVRInput.Controller.None)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
                {
                    grabPressed = true;
                }
            }
            else
            {
                // Fallback: Check either hand trigger if we can't identify the hand side
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) ||
                    OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
                {
                    grabPressed = true;
                }
            }

            // Standard Input / Debug
            if (Input.GetButtonDown(grabButtonName) || Input.GetKeyDown(debugKey))
            {
                grabPressed = true;
            }

            if (grabPressed)
            {
                SpawnInHand(currentHand);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the hand
        if (other.CompareTag(handTag))
        {
            isHandInside = true;
            currentHand = other.gameObject;
            Debug.Log("Hand entered spawn area: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is the current hand
        if (other.CompareTag(handTag) && other.gameObject == currentHand)
        {
            isHandInside = false;
            currentHand = null;
            Debug.Log("Hand exited spawn area.");
        }
    }

    /// <summary>
    /// Sets the index of the prefab to spawn next.
    /// Call this from another script to change the active ingot.
    /// </summary>
    /// <param name="index">Index of the prefab in the ingotPrefabs array.</param>
    public void SetNextPrefab(int index)
    {
        if (index >= 0 && index < ingotPrefabs.Length)
        {
            currentPrefabIndex = index;
            Debug.Log("Next ingot set to index: " + index);
        }
        else
        {
            Debug.LogWarning("Invalid prefab index: " + index);
        }
    }

    /// <summary>
    /// Spawns the ingot prefab and attaches it to the specified hand.
    /// Can be called via signal or event.
    /// </summary>
    /// <param name="hand">The hand GameObject to attach the ingot to.</param>
    public void SpawnInHand(GameObject hand)
    {
        if (ingotPrefabs != null && ingotPrefabs.Length > 0 && hand != null)
        {
            if (currentPrefabIndex < 0 || currentPrefabIndex >= ingotPrefabs.Length)
            {
                Debug.LogWarning("Current prefab index is out of range. Resetting to 0.");
                currentPrefabIndex = 0;
            }

            GameObject prefabToSpawn = ingotPrefabs[currentPrefabIndex];

            if (prefabToSpawn != null)
            {
                // Instantiate the prefab at the hand's position and rotation
                GameObject spawnedIngot = Instantiate(prefabToSpawn, hand.transform.position, hand.transform.rotation);
                
                // Parent it to the hand so it moves with it
                spawnedIngot.transform.SetParent(hand.transform);

                // If the ingot has a Rigidbody, set it to kinematic so it doesn't fall immediately
                Rigidbody rb = spawnedIngot.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                Debug.Log("Spawned " + spawnedIngot.name + " in hand " + hand.name);
            }
            else
            {
                Debug.LogWarning("Prefab at index " + currentPrefabIndex + " is null.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot spawn ingot: Prefab list is empty or Hand is null.");
        }
    }
}
