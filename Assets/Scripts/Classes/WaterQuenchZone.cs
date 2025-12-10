using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterQuenchZone : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem steamParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            // Play steam
            if (steamParticles != null)
                steamParticles.Play();

            other.GetComponent<SwordQuenchable>()?.TryBeginQuench();
        }
    }

}
