using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityCalculator : MonoBehaviour
{
    [SerializeField] private GamePhase gamePhase;
    [SerializeField] private float ForgeScore = 0;
    [SerializeField] private float SmithScore = 0;

    // Start is called before the first frame update
   public float CalculateScore()
    {
        if(gamePhase.Finish == 1)
        {
            ForgeScore = gamePhase.ForgePoints;
            SmithScore = gamePhase.SmithPoints;
            return ForgeScore + SmithScore;
        }
        else
        {
            return 0;
        }
    }

    public int qualityLookup(float score)
    {
       if(score >= 0 && score <= 66)
        {
            return 1;
        }else if(score > 66 && score <= 132)
        {
            return 2;
        }
        else
        {
            return 3;
        }


           
    }
}
