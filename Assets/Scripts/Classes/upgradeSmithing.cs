using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeSmithing : MonoBehaviour
{

    public float minVelocity = 1.5f;       // Minimum velocity for scoring
    public float maxVelocity = 5f;         // Max velocity for scaling score
    public float extraHitPenalty = 5f;     // Penalty per extra hit

    public void setminVelocity(float vel) {  minVelocity = vel; }
    public void setmaxVelocity(float vel) { maxVelocity = vel; }

    public void setExtraHitPenalty(float vel) { extraHitPenalty = vel; }

    public float getminVelocity(float vel) { return minVelocity; }
    public float getMaxVelocity(float vel) {return maxVelocity; }

    public float getExtraHitPenalty(float vel) { return extraHitPenalty; }
}
