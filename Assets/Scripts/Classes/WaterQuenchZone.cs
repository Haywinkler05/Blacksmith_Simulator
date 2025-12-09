using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterQuenchZone : MonoBehaviour
{
    [Header("Scoring")]
    public int forgeScore = 0; // Set by StateMachine externally

    [Header("Settings")]
    public float requiredTimeInside = 2f; // Must stay 2 seconds in water

    private SwordQuenchable currentSword = null;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        SwordQuenchable sword = other.GetComponent<SwordQuenchable>();

        if (sword != null)
        {
            currentSword = sword;
            timer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentSword == null) return;

        if (other.GetComponent<SwordQuenchable>() != currentSword) return;

        timer += Time.deltaTime;

        // If sword has stayed long enough — start quenching
        if (timer >= requiredTimeInside)
        {
            currentSword.BeginQuench(forgeScore);
            currentSword = null; // stop repeated triggering
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentSword != null && other.GetComponent<SwordQuenchable>() == currentSword)
        {
            // Reset if sword leaves the water too early
            currentSword = null;
            timer = 0f;
        }
    }
}
