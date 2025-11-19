using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public event Action<HitInfo> onHammerHit;
    private int NumofHits = 0;
    private Vector3 hammerVel;
    [SerializeField]
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (rb != null) { 
            hammerVel = rb.velocity; 
        
        
        }
    }
    public struct HitInfo
    {
        public bool anvilHit;
        public Vector3 hammerVel;
    }

   
    public int GetNumOfHits() //This can be used in our smithing script to check the amount of hits needed for the smithing to be done
    {
        return this.NumofHits;
    }

    public Vector3 GetHammerVel()
    {
        return this.hammerVel;
    }


    private void OnCollisionEnter(Collision collision)
    {
        bool anvilHit = collision.gameObject.CompareTag("Anvil");
        NumofHits++;

        HitInfo info = new HitInfo { anvilHit = anvilHit, hammerVel = hammerVel};
        
        onHammerHit?.Invoke(info);
    }
}


