using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Hammer hammerClass;
    public GameObject hammer;
    public GameObject anvil;

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
            anvil.GetComponent<AudioSource>().Play();
        }
    }
    public void Update()
    {

    }
}
