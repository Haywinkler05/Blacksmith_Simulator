using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeStage : MonoBehaviour
{
    private StateMachine stateMachine;

    [Header("Forge Settings")]
    [SerializeField] private float perfectWindowStart = 4f;
    [SerializeField] private float perfectWindowEnd = 6f;
    [SerializeField] private float maxHeatTime = 10f;

    [Header("Quality Scores")]
    [SerializeField] private int perfectScore = 25;
    [SerializeField] private int goodScore = 18;
    [SerializeField] private int poorScore = 10;

    [Header("VR Objects")]
    [SerializeField] private Transform forgeZone;
    [SerializeField] private GameObject metalPiece;
    [SerializeField] private float forgeRadius = 0.5f;
    void Awake()
    {
        stateMachine = GetComponent<StateMachine>(); // Or FindObjectOfType
        enabled = false; // Start disabled
    }

    public void StartStage()
    {
        // Initialize the minigame
        Debug.Log("Forge stage started!");
    }

    void Update()
    {
        // Your forge minigame logic here
        //When you place the material in forge, start timer
        //Have to pull out material at a certain threshold
        //undersmelt and oversmelt give less quality score
        //Perfect score gives 25 out of 75

        // When done:
        // stateMachine.CompleteCurrentStage(qualityScore);
    }

}
