using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindstoneController : MonoBehaviour
{
    [Header("Spin Settings")]
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

        // Rotate around an arbitrary axis without reparenting
        transform.Rotate(spinAxis.normalized, currentSpeed * Time.deltaTime, Space.World);
    }

    public void StartSpinning() => shouldSpin = true;
    public void StopSpinning() => shouldSpin = false;

    private void OnCollisionEnter(Collision collision)
    {
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
