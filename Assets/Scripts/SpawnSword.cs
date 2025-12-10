using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpawnSword : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The list of prefabs available to spawn.")]
    public GameObject[] swordPrefabs;

    [Tooltip("The tag used to identify the player's hand.")]
    public string handTag = "PlayerHand";

    [Tooltip("Key for testing in editor without VR.")]
    public KeyCode debugKey = KeyCode.G;

    private GameObject currentHand;
    public GameObject currentSpawnedSword;
    private bool isHandInside = false;
    private bool wasGripping = false;
    private bool wasDeletePressed = false;

    // Update is called once per frame
    void Update()
    {
        // Check for Delete Input
        // User requested "X on Right Controller". On Quest, X is on Left, A is on Right (both are Primary).
        // We will check Primary Button on BOTH hands to cover all bases.
        bool deletePressed = false;
        
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary) && rightPrimary)
            deletePressed = true;

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary) && leftPrimary)
            deletePressed = true;

        if (deletePressed || currentSpawnedSword == null)
        {
            if (!wasDeletePressed)
            {
                if (currentSpawnedSword != null)
                {
                    Destroy(currentSpawnedSword);
                    currentSpawnedSword = null;
                    Debug.Log("Sword destroyed via input.");
                }
                wasDeletePressed = true;
            }
        }
        else
        {
            wasDeletePressed = false;
        }

        if (isHandInside && currentHand != null)
        {
            bool grabPressed = false;

            // Determine Hand Node
            XRNode node = XRNode.RightHand; // Default to Right
            if (currentHand.name.Contains("Left") || currentHand.name.Contains("LHand"))
            {
                node = XRNode.LeftHand;
            }

            // Get Input Device
            InputDevice device = InputDevices.GetDeviceAtXRNode(node);
            float gripValue;

            // Check Grip Value (Threshold 0.5)
            if (device.TryGetFeatureValue(CommonUsages.grip, out gripValue))
            {
                if (gripValue > 0.5f)
                {
                    if (!wasGripping)
                    {
                        grabPressed = true;
                        wasGripping = true;
                    }
                }
                else
                {
                    wasGripping = false;
                }
            }

            // Debug Key for testing in Editor
            if (Input.GetKeyDown(debugKey))
            {
                grabPressed = true;
            }

            if (grabPressed)
            {
                SpawnRandomSword(currentHand);
            }
        }
        else
        {
            wasGripping = false;
        }
    }
    public void deleteSword()
    {

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
    /// Spawns a random sword prefab and attaches it to the specified hand.
    /// </summary>
    /// <param name="hand">The hand GameObject to attach the sword to.</param>
    public void SpawnRandomSword(GameObject hand)
    {
        // Check if a sword already exists
        if (currentSpawnedSword != null)
        {
            Debug.Log("Cannot spawn new sword: Previous sword still exists.");
            return;
        }

        if (swordPrefabs != null && swordPrefabs.Length > 0 && hand != null)
        {
            int randomIndex = Random.Range(0, swordPrefabs.Length);
            GameObject prefabToSpawn = swordPrefabs[randomIndex];

            if (prefabToSpawn != null)
            {
                // Instantiate the prefab at the hand's position and rotation
                GameObject spawnedSword = Instantiate(prefabToSpawn, hand.transform.position, hand.transform.rotation);
                currentSpawnedSword = spawnedSword;
                
                // We do not parent the sword or set it to kinematic.
                // This allows existing grab mechanics to interact with it, or for it to fall naturally.

                Debug.Log("Spawned " + spawnedSword.name + " at " + hand.name);
            }
            else
            {
                Debug.LogWarning("Prefab at index " + randomIndex + " is null.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot spawn sword: Prefab list is empty or Hand is null.");
        }
    }
}
