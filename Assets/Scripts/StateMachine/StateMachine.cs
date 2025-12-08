using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum GameState
    {
        Forge,
        Smithing,
        Quench,
        Finished
    }

    [SerializeField] private GameState currentState;
    [SerializeField] private GameObject forgeStage;
    [SerializeField] private GameObject smithingStage;
    [SerializeField] private GameObject quenchStage;
    [SerializeField] private GameObject finishedStage;

    private int currentBladeQuality = 0; //



    void Start()
    {
        TransitionToState(GameState.Forge);
    }

    public void TransitionToState(GameState newState)
    {
        // Deload current stage
        DeloadCurrentStage();

        // Set new state
        currentState = newState;

        // Load new stage
        LoadCurrentStage();

        Debug.Log($"Transitioned to: {currentState}");
    }
    private void LoadCurrentStage()
    {
        switch (currentState)
        {
            case GameState.Forge:
                if (forgeStage != null) forgeStage.SetActive(true);
                break;
            case GameState.Smithing:
                if (smithingStage != null) smithingStage.SetActive(true);
                break;
            case GameState.Quench:
                if (quenchStage != null) quenchStage.SetActive(true);
                break;
            case GameState.Finished:
                if (finishedStage != null) finishedStage.SetActive(true);
                break;
        }
    }

    private void DeloadCurrentStage()
    {
        // Disable all stages
        if (forgeStage != null) forgeStage.SetActive(false);
        if (smithingStage != null) smithingStage.SetActive(false);
        if (quenchStage != null) quenchStage.SetActive(false);
        if (finishedStage != null) finishedStage.SetActive(false);
    }
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
                // Reset for next sword
                TransitionToState(GameState.Forge);
                break;
        }
    }

    public int GetCurrentBladeQuality()
    {
        return currentBladeQuality;
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}
