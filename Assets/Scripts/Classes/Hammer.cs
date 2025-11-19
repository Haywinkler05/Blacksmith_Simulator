using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
   
    private int NumofHits = 0;
    private float hammerVel;


    public struct HitInfo
    {
        public bool anvilHit;
        public float hammerVel;
    }

   
    public int GetNumOfHits() //This can be used in our smithing script to check the amount of hits needed for the smithing to be done
    {
        return this.NumofHits;
    }

    public float GetHammerVel()
    {
        return this.hammerVel;
    }

    public void calculateHammerVel()
    {
        //calculate the hammer velocity
    }
    public HitInfo HammerHit(bool anvilHit)
    {
        if (anvilHit) NumofHits++;

        return new HitInfo { anvilHit = anvilHit, hammerVel = hammerVel };
    }
}


