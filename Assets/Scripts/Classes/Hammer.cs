using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammer;
    private int NumofHits = 0;

    public void SetHammer(GameObject hammer)
    {
        this.hammer = hammer;
    }
    public GameObject GetHammer()
    {
        return this.hammer;
    }
    public int GetNumOfHits() //This can be used in our smithing script to check the amount of hits needed for the smithing to be done
    {
        return this.NumofHits;
    }
    public void HammerHit(bool goodHit)
    {
        if (goodHit)
        {
            //Play good hit sound
            //Do changes to the sword game object
            //Increase quality
            //Increase # of hits
        }
        else
        {
            //Play bad hit sound
            //Do changes to sword game object
            //Decrease quality
            //Increase # of hits
        }
    }
}


