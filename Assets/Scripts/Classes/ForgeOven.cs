using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeOven : MonoBehaviour
{
    public bool shouldBurn;

    void Update()
    {
        shouldBurn = GamePhase.Instance.Forge == 1;

        // visual logic
        if (shouldBurn)
        {
            // turn on fire animation etc
        }
    }
}
