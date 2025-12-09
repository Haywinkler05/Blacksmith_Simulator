using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordQuenchable : MonoBehaviour
{
    [Header("References")]
    public GameObject rawIronObject;        // The pre-sword iron bar
    public GameObject finishedSwordObject;  // The sword mesh that shows after quenching
    public ParticleSystem steamParticles;

    public SwordBender swordBender;         // Bending script
    public SwordBurn swordBurn;             // Burn/material script
    private StateMachine stateMachine;       // So we can notify the game flow

    [Header("Settings")]
    public float quenchTime = 2f;           // Time spent in water before transformation

    private bool isQuenching = false;

    void Awake()
    {
        stateMachine = FindObjectOfType<StateMachine>();
    }


    public void BeginQuench(int finalQualityScore)
    {
        if (isQuenching) return;

        StartCoroutine(QuenchRoutine(finalQualityScore));
    }

    private IEnumerator QuenchRoutine(int finalQuality)
    {
        isQuenching = true;

        // Play steam effect
        if (steamParticles != null)
            steamParticles.Play();

        // Wait inside the water
        yield return new WaitForSeconds(quenchTime);

        // Hide the iron bar, show the sword
        if (rawIronObject != null) rawIronObject.SetActive(false);
        if (finishedSwordObject != null) finishedSwordObject.SetActive(true);

        // Apply bending + burn
        if (swordBender != null)
            swordBender.ApplyBendsFromScore(finalQuality);

        if (swordBurn != null)
            swordBurn.ApplyBurnFromScore(finalQuality);

        // Notify state machine that quenching is done
        if (stateMachine != null)
            stateMachine.CompleteCurrentStage(finalQuality);

        isQuenching = false;
    }
}
