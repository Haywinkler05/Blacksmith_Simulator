using System.Collections;
using UnityEngine;

public class SwordQuenchable : MonoBehaviour
{
    [Header("References")]
    public GameObject rawIronObject;        // Before transformation
    public GameObject finishedSwordObject;  // After transformation

    public SwordBender swordBender;         // Bending logic
    public SwordBurn swordBurn;             // Burn/material logic

    [Header("Settings")]
    public float quenchTime = 2f;           // Time spent in water

    private bool isQuenching = false;

    public void TryBeginQuench()
    {
        // Only quench if global state says we are in the quench phase
        if (GamePhase.Instance.Quench != 1)
            return;

        if (isQuenching) return;

        float forge = GamePhase.Instance.ForgePoints;
        float smith = GamePhase.Instance.SmithPoints;

        StartCoroutine(QuenchRoutine(forge, smith));

        
    }

    private IEnumerator QuenchRoutine(float f, float s)
    {
        isQuenching = true;


        // Wait inside water
        yield return new WaitForSeconds(quenchTime);

        // Switch iron → sword
        if (rawIronObject != null)
            rawIronObject.SetActive(false);

        if (finishedSwordObject != null)
            finishedSwordObject.SetActive(true);

        // Apply bending based on combined forge + smith errors
        if (swordBender != null)
            swordBender.ApplyBendsFromScore((int)s);

        // Apply burning char based on total heat mismanagement
        if (swordBurn != null)
            swordBurn.ApplyBurnFromScore((int)f);

        isQuenching = false;
        GamePhase.Instance.SetPhaseGrind();
    }
}
