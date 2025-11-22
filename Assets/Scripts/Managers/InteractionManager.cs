using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Hammer hammerClass;
    public GameObject hammer;
    public GameObject anvil;
    public float vel;
 

    public void Start()
    {
        if (hammer != null)
        {
            hammerClass = hammer.GetComponent<Hammer>();
            if (hammerClass != null)
            {
                hammerClass.onHammerHit += HandleHammerHit;
            }
        }
    }
    private void HandleHammerHit(Hammer.HitInfo info)
    {
        if (info.anvilHit)
        {
            vel = info.hammerVel.magnitude;
            Debug.Log("The velocity is " + vel);
            if (vel > 10f)
            {
                anvil.GetComponent<AudioSource>().Play();
            }
        }
    }
    public void Update()
    {

    }
}
