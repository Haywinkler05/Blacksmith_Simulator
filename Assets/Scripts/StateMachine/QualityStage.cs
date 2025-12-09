using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityStage : MonoBehaviour
{
    private StateMachine stateMachine;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>(); // Or FindObjectOfType
        enabled = false; // Start disabled
    }

    public void StartStage()
    {
        // Initialize the minigame
        Debug.Log("Quality stage started!");
    }

    void Update()
    {
        // Your quality minigame logic here

        // When done:
        // stateMachine.CompleteCurrentStage(qualityScore);
    }

}
