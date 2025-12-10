using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindstoneController : MonoBehaviour
{
    [Header("Spin Settings")]
    public Transform axis;    // Drag your GrindstoneAxis object here
    public float maxSpinSpeed = 720f;
    public float accelerationTime = 3f;
    public float decelerationTime = 3f;

    [Header("Axis of Spin (world space)")]
    public Vector3 spinAxis = new Vector3(1, 0, 0); // set to your grindstone's real axis

    [Header("Particles")]
    public ParticleSystem grindParticles;

    private int shouldSpin;

    private float currentSpeed = 0f;

    void Update()
    {
        shouldSpin = GamePhase.Instance.Grind;
        // Determine acceleration and deceleration rates
        float accelRate = maxSpinSpeed / accelerationTime;
        float decelRate = maxSpinSpeed / decelerationTime;

        currentSpeed = (shouldSpin == 1)
            ? Mathf.MoveTowards(currentSpeed, maxSpinSpeed, accelRate * Time.deltaTime)
            : Mathf.MoveTowards(currentSpeed, 0f, decelRate * Time.deltaTime);

        // Rotate around the reference pivot
        if (axis != null)
        {
            transform.RotateAround(axis.position, spinAxis, currentSpeed * Time.deltaTime);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            Debug.Log("Colliding with sword");
        }
        // Play or stop particles based on spinning
        if (grindParticles != null)
        {
            if (currentSpeed > 0f && !grindParticles.isPlaying)
                grindParticles.Play();
            else if (currentSpeed == 0f && grindParticles.isPlaying)
                grindParticles.Stop();
        }
    }
}
