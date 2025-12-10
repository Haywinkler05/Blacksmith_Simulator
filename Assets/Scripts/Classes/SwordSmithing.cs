using System.Collections;
using UnityEngine;

public class SwordSmithing : MonoBehaviour
{
    [Header("Global Settings")]
    public int targetHits = 10;                // Hits before penalty starts
    public float minVelocity = 1.5f;          // Minimum velocity for scoring
    public float maxVelocity = 5f;            // Max velocity for scaling score
    public float extraHitPenalty = 5f;        // Penalty per extra hit

    [Header("Scoring")]
    public float maxHitScore = 10;         // Max score for one hit

    private float smithScore = 0f;
    private bool isSmithing = false;
    private int hitCount = 0;

    [Header("Audio")]
    public AudioSource audioSource;           // Whistle audio source
    public AudioClip whistleClip;             // Whistle plays once
    public AudioClip goodHitSFX;
    public AudioClip badHitSFX;

    void Update()
    {
        if (!isSmithing || GamePhase.Instance.Smith == 0) return;

        // Smithing ends automatically if whistle finished
        if (audioSource != null && !audioSource.isPlaying && audioSource.clip == whistleClip)
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
        isSmithing = false;

        // Clamp final score between 0 and 100
        smithScore = Mathf.Clamp(smithScore, 0, 100);
        GamePhase.Instance.SmithPoints += Mathf.RoundToInt(smithScore);

        Debug.Log($"SMITHING FINISHED | Total Score: {smithScore}");

        // Stop audio if still playing
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

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

        // Play whistle once
        if (audioSource != null && whistleClip != null)
        {
            audioSource.clip = whistleClip;
            audioSource.loop = false;
            audioSource.Play();
        }
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
