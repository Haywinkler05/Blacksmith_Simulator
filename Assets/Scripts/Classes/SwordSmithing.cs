using System.Collections;
using UnityEngine;

public class SwordSmithing : MonoBehaviour
{
    [Header("Global Settings")]
    public int targetHits = 10;            // Hits before penalty starts
    public float minVelocity = .1f;       // Minimum velocity for scoring
    public float maxVelocity = 1.5f;         // Max velocity for scaling score
    public float extraHitPenalty = 5f;     // Penalty per extra hit
    public float smithDuration = 30f;      // Duration of smithing stage in seconds

    [Header("Scoring")]
    public float maxHitScore = 10f;        // Max score for one hit

    private float smithScore = 0f;
    private bool isSmithing = false;
    private int hitCount = 0;
    private float smithTimer = 0f;

    [Header("Audio SFX")]
    public AudioSource audioSource;        // Optional hammer SFX source
    public AudioClip goodHitSFX;
    public AudioClip badHitSFX;

    void Update()
    {
        if (!isSmithing || GamePhase.Instance.Smith == 0) return;

        // Update smithing timer
        smithTimer += Time.deltaTime;

        // End smithing automatically when duration finishes
        if (smithTimer >= smithDuration)
        {
            FinishSmithing();
        }
    }

    /// <summary>
    /// Call this when the hammer hits the anvil. Pass the hammer's current velocity magnitude.
    /// </summary>
    public void RegisterHammerHit(float hammerVelocity)
    {
        if (!isSmithing) return;

        hitCount++;

        if (hitCount <= targetHits)
        {
            // Smooth scoring based on velocity
            float clampedVelocity = Mathf.Clamp(hammerVelocity, minVelocity, maxVelocity);
            float normalized = (clampedVelocity - minVelocity) / (maxVelocity - minVelocity);
            float score = Mathf.Lerp(0f, maxHitScore, normalized);

            smithScore += score;
            PlaySFX(goodHitSFX);
        }
        else
        {
            // Penalize extra hits beyond target
            smithScore -= extraHitPenalty;
            PlaySFX(badHitSFX);
        }

        Debug.Log($"Hit {hitCount} | Current Score: {smithScore}");
    }

    private void FinishSmithing()
    {
        if (!isSmithing) return;

        isSmithing = false;

        // Clamp final score between 0 and 100
        smithScore = Mathf.Clamp(smithScore, 0, 100);
        GamePhase.Instance.SmithPoints += Mathf.RoundToInt(smithScore);

        Debug.Log($"SMITHING FINISHED | Total Score: {smithScore}");

        // Trigger next game phase
        GamePhase.Instance.SetPhaseQuench();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Smith") || GamePhase.Instance.Smith == 0) return;

        StartSmithing();
    }

    private void StartSmithing()
    {
        Debug.Log("SMITHING STARTED");

        isSmithing = true;
        smithScore = 0f;
        hitCount = 0;
        smithTimer = 0f;

        // Audio handled by anvil; no local audio playback needed here
    }

    private void PlaySFX(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public float GetSmithScore()
    {
        return smithScore;
    }
}
