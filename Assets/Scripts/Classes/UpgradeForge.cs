using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeForge : MonoBehaviour
{
    public float yellowTime = 2f;          // When the sword reaches yellow heat
    public float orangeTime = 5f;          // PERFECT heat moment
    public float pinkTime = 7f;            // Overheated / burned


    public void setYellowTime(float time) { yellowTime = time;}
    public void setorangeTime(float time) { yellowTime = time; }
    public void setpinkTime(float time) { yellowTime = time; }


    public float getYellowTime() { return yellowTime; }

    public float getOrangeTime() { return orangeTime; }

    public float getPinkTime() { return pinkTime; }
}
