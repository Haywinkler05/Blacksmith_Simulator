using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordForge : MonoBehaviour
{
    [Header("Renderer Target")]
    public Renderer swordRenderer;   // Drag the child mesh here

    [Header("Forge Materials (Drag & Drop)")]
    public Material baseMaterial;
    public Material yellowMaterial;
    public Material orangeMaterial;   // Ideal heat
    public Material pinkMaterial;

    [Header("Timing Settings")]
    public float yellowTime = 2f;          // When the sword reaches yellow heat
    public float orangeTime = 5f;          // PERFECT heat moment
    public float pinkTime = 7f;            // Overheated / burned

    [Header("Scoring Settings")]
    public float penaltyPerSecond = 20f;   // How fast score drops from perfect

    private float forgeTimer = 0f;
    private int isForging;

    private float forgeScore = 0f;
    private bool isSwordInForge = false;

    void Start()
    {
        if (swordRenderer == null)
            swordRenderer = GetComponentInChildren<Renderer>();

        if (swordRenderer == null)
            Debug.LogError("SwordForge: No renderer assigned or found!");

        swordRenderer.material = baseMaterial;

        // SAFE to access GamePhase now
        isForging = GamePhase.Instance.Forge;
    }

    void Update()
    {
        if (isForging == 0 || !isSwordInForge) return;

        forgeTimer += Time.deltaTime;
        UpdateHeatMaterial();
    }

    private void UpdateHeatMaterial()
    {
        if (forgeTimer >= pinkTime)
        {
            swordRenderer.material = pinkMaterial;
        }
        else if (forgeTimer >= orangeTime)
        {
            swordRenderer.material = orangeMaterial;
        }
        else if (forgeTimer >= yellowTime)
        {
            swordRenderer.material = yellowMaterial;
        }
        else
        {
            swordRenderer.material = baseMaterial;
        }
    }

    private void FinishForge()
    {
        float difference = Mathf.Abs(forgeTimer - orangeTime);
        forgeScore = Mathf.Clamp(100 - (difference * penaltyPerSecond), 0, 100);

        Debug.Log($"Forge Score: {forgeScore} (Timer {forgeTimer:F2}s)");

        forgeTimer = 0f;
        isSwordInForge = false;

        GamePhase.Instance.ForgePoints += forgeScore;
        GamePhase.Instance.SetPhaseSmith();
    }

    // Trigger entry starts the mini-game
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Forge") && (GamePhase.Instance.Forge == 1))
        {
            StartForging();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Forge") && isSwordInForge)
        {
            FinishForge();
        }
    }

    private void StartForging()
    {
        forgeTimer = 0f;
        isSwordInForge = true;

        swordRenderer.material = baseMaterial;
    }

    public float GetForgeScore()
    {
        return forgeScore;
    }
}
