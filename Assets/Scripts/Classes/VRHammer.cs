using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHammer : MonoBehaviour
{
    public string swordTag = "Sword"; // Tag for the sword
    private Rigidbody rb;


    [Header("Audio Feedback")]
    public AudioSource goodHitAudio;
    public AudioSource badHitAudio;

    [Header("Velocity Thresholds")]
    public float minGoodVelocity = 1.5f; // Minimum velocity for a good hit
    public float minBadVelocity = 0.1f;  // Anything below this is definitely a bad hit

    [Header("References")]
    public WeaponGrabber grabber;   // <-- Drag your hammer's WeaponGrabber here


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(swordTag))
        {
            // Hammer velocity magnitude
            float hammerVelocity = grabber.GetSwingVelocity();


            // Call the sword's RegisterHammerHit function
            SwordSmithing sword = collision.gameObject.GetComponent<SwordSmithing>();
            if (sword != null)
            {
                sword.RegisterHammerHit(hammerVelocity);
            }

            // Decide which audio to play
            if (hammerVelocity >= minGoodVelocity)
            {
                if (goodHitAudio != null) goodHitAudio.Play();
            }
            else
            {
                if (badHitAudio != null) badHitAudio.Play();
            }

            // Debug
            Debug.Log($"Hit sword with velocity {hammerVelocity}");
        }
    }
}
