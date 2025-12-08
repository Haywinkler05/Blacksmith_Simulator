using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // State enum
    public enum GameState
    {
        Forge,
        Smithing,
        Quench,
        Finished
    }

    [SerializeField] private GameState currentState;

    // Reference to stage scripts
    [SerializeField] private ForgeStage forgeStage;
    [SerializeField] private SmithingStage smithingStage;
    [SerializeField] private QualityStage quenchStage;
    [SerializeField] private FinishedStage finishedStage;

    // Current blade quality tracking
    private int currentBladeQuality = 0;

    void Start()
    {
        TransitionToState(GameState.Forge);
    }

    public void TransitionToState(GameState newState)
    {
        // Deactivate current stage
        DeactivateCurrentStage();

        // Set new state
        currentState = newState;

        // Activate new stage
        ActivateCurrentStage();

        Debug.Log($"Transitioned to: {currentState}");
    }

    private void ActivateCurrentStage()
    {
        switch (currentState)
        {
            case GameState.Forge:
                if (forgeStage != null)
                {
                    forgeStage.enabled = true;
                    forgeStage.StartStage(); // Call initialization method
                }
                break;
            case GameState.Smithing:
                if (smithingStage != null)
                {
                    smithingStage.enabled = true;
                    smithingStage.StartStage();
                }
                break;
            case GameState.Quench:
                if (quenchStage != null)
                {
                    quenchStage.enabled = true;
                    quenchStage.StartStage();
                }
                break;
            case GameState.Finished:
                if (finishedStage != null)
                {
                    finishedStage.enabled = true;
                    finishedStage.StartStage();
                }
                break;
        }
    }

    private void DeactivateCurrentStage()
    {
        // Disable all stage scripts
        if (forgeStage != null) forgeStage.enabled = false;
        if (smithingStage != null) smithingStage.enabled = false;
        if (quenchStage != null) quenchStage.enabled = false;
        if (finishedStage != null) finishedStage.enabled = false;
    }

    // Called by stage scripts when they finish
    public void CompleteCurrentStage(int qualityScore)
    {
        currentBladeQuality = qualityScore;

        switch (currentState)
        {
            case GameState.Forge:
                TransitionToState(GameState.Smithing);
                break;
            case GameState.Smithing:
                TransitionToState(GameState.Quench);
                break;
            case GameState.Quench:
                TransitionToState(GameState.Finished);
                break;
            case GameState.Finished:
                // Tell OrderSystem we completed an order
                FindObjectOfType<OrderSystem>().CompleteOrder();
                // Reset for next sword
                TransitionToState(GameState.Forge);
                break;
        }
    }

    public int GetCurrentBladeQuality() => currentBladeQuality;
    public GameState GetCurrentState() => currentState;
}