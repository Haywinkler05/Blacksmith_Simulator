using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhase : MonoBehaviour
{
    // Singleton access
    public static GamePhase Instance;

    [Header("Global Phase Flags (0 or 1)")]
    public int Forge = 0;
    public int Smith = 0;
    public int Quench = 0;
    public int Grind = 0;

    private void Awake()
    {
        // Basic singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Helper to reset all to zero
    public void ResetPhases()
    {
        Forge = Smith = Quench = Grind = 0;
    }

    // Helper to activate one phase
    public void SetPhaseForge() { ResetPhases(); Forge = 1; }
    public void SetPhaseSmith() { ResetPhases(); Smith = 1; }
    public void SetPhaseQuench() { ResetPhases(); Quench = 1; }
    public void SetPhaseGrind() { ResetPhases(); Grind = 1; }

    // Optionally: Serialize events later if needed
}
