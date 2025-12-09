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
    public Vector3 spinAxis = new Vector3(1, 0, 0); // <-- set to your grindstone's real axis

    [Header("Particles")]
    public ParticleSystem grindParticles;

    private float currentSpeed = 0f;
    private bool shouldSpin = false;

    void Update()
    {
        // Smooth ease in/out
        float accelRate = maxSpinSpeed / accelerationTime;
        float decelRate = maxSpinSpeed / decelerationTime;

        currentSpeed = shouldSpin
            ? Mathf.MoveTowards(currentSpeed, maxSpinSpeed, accelRate * Time.deltaTime)
            : Mathf.MoveTowards(currentSpeed, 0f, decelRate * Time.deltaTime);

        // Rotate around the reference pivot
        if (axis != null)
        {
            transform.RotateAround(axis.position, spinAxis, currentSpeed * Time.deltaTime);
        }
    }

    public void StartSpinning() => shouldSpin = true;
    public void StopSpinning() => shouldSpin = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            Debug.Log("Colliding with sword");
        }
        if (collision.collider.CompareTag("Sword"))
        {
            if (grindParticles != null) grindParticles.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
       
        if (collision.collider.CompareTag("Sword"))
        {
            if (grindParticles != null) grindParticles.Stop();
        }
    }
}
