using System.Collections;
using UnityEngine;

public class SwordGrind : MonoBehaviour
{
    [Header("Grinding Settings")]
    public float grindDuration = 6f;         // Grind time after first touch
    public float smithGainPerSecond = 5f;    // How many smith points added per second
    public float rebendThreshold = 20f;      // Rebend after every +20 points

    [Header("References")]
    public SwordBender swordBender;          // ApplyBendsFromScore()


    private bool isGrinding = false;
    private float grindTimer = 0f;
    private float lastRebendScore = 0f;      // Used to track intervals

    private bool grindStarted = false;


    private void OnTriggerEnter(Collider other)
    {
        // Only activate if touching the grindstone during grind phase
        if (!other.CompareTag("GrindStone")) return;
        if (GamePhase.Instance.Grind != 1) return;

        if (!grindStarted)
        {
            grindStarted = true;

            grindTimer = 0f;
            lastRebendScore = GamePhase.Instance.SmithPoints;

        }

        isGrinding = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrindStone"))
        {
            isGrinding = false;
        }
    }


    private void Update()
    {
        if (!grindStarted) return;
        if (GamePhase.Instance.Grind != 1) return;

        grindTimer += Time.deltaTime;

        // Stop grinding when time is up
        if (grindTimer >= grindDuration)
        {
            EndGrinding();
            return;
        }

        if (isGrinding)
        {
            // Add smithing points based on time
            GamePhase.Instance.SmithPoints += smithGainPerSecond * Time.deltaTime;

            // Check if we passed another rebend threshold
            float currentSmith = GamePhase.Instance.SmithPoints;
            if (currentSmith - lastRebendScore >= rebendThreshold)
            {
                lastRebendScore = currentSmith;

                if (swordBender != null)
                    swordBender.ApplyBendsFromScore((int)currentSmith);
            }
        }
    }
    private void EndGrinding()
    {
        grindStarted = false;
        isGrinding = false;

        // Move to next phase
        GamePhase.Instance.SetPhaseFinish();  // Or SetPhaseComplete() — whichever you use next
    }
}


