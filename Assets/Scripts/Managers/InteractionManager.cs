using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Hammer hammerClass;
    public GameObject hammer;


    public void Start()
    {
        hammerClass = new Hammer();
        hammerClass.SetHammer(hammer);
    }

    public void Update()
    {

    }
}
