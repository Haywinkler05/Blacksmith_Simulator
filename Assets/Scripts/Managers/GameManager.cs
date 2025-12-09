using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Order system
        //Set number of orders a certain day
        //Some orders have high quality expectations, or low quality.
        //Timer to complete orders maybe?

    //Mini Game functions will be called from here


    //Forge mini game
        //Set state machine to forging
        //Player has to leave metal in and heat it up to set temp. Randomized temp?
        //Player then needs to take out metal once it reaches a certain color
        //Set state machine to smithing 

    //Smithing mini game
        //Use math to calculate BPM, making the player match hammer hits to the beats of the song
        //Get a random number of hits with a certian threshold
        //If hit off beat, twist blade out of shape
        //Else, hit it into shape a bit

    //Quench + Dummy
        //Simple mini game, have player plunge finished sword into quenching barrel
        //Read out quality of sword when hit against dummy, has to match quality of order

    //Grindstone
        //Simple mini game where player lays sword on grindstone to sharpen
        //Too much of the grindstone will break quality, has to get it just right
}
