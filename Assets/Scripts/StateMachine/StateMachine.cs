using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    private ForgeStage stage1;
    private SmithingStage stage2;
    private QualityStage stage3;
    private FinishedStage stage4;
    private string currentStage;

    public void loadStage()
    {
        //We would load the stage here
    }

    public void deloadStage()
    {
        //Deload stage
    }
}
