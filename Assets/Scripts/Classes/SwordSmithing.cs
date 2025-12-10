using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSmithing : MonoBehaviour
{
    [Header("Global Settings")]
    public int hammerTargetBeats = 6;
    public float beatWindow = 0.25f;
    public float beatInterval = 1.2f;           // 100 BPM every other beat

    [Header("Scoring")]
    public float perfectHitValue = 16.6f;
    public float goodHitValue = 10f;
    public float badHitValue = 0f;

    private float smithScore = 0f;
    private bool isSmithing = false;
    private float beatTimer = 0f;
    private int currentBeat = 0;
    private bool insideSmithZone = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip smithingBeatTrack;   // 100 BPM song
    public AudioClip goodHitSFX;
    public AudioClip badHitSFX;

    public float beatTrackDelay = 3f;


    void Update()
    {
        if (GamePhase.Instance.Smith == 0) return;
        if (!isSmithing) return;

        beatTimer += Time.deltaTime;

        if (beatTimer >= beatInterval)
        {
            beatTimer = 0f;
            currentBeat++;

            if (currentBeat >= hammerTargetBeats)
                FinishSmithing();
        }
    }

    public void RegisterHammerHit()
    {
        if (!isSmithing) return;

        float difference = Mathf.Abs(beatTimer - (beatInterval / 2));

        if (difference < beatWindow)
        {
            smithScore += perfectHitValue;
            PlaySFX(goodHitSFX);
        }
        else if (difference < beatWindow * 2)
        {
            smithScore += goodHitValue;
            PlaySFX(goodHitSFX);
        }
        else
        {
            smithScore += badHitValue;
            PlaySFX(badHitSFX);
        }
    }

    private void FinishSmithing()
    {
        isSmithing = false;

        smithScore = Mathf.Clamp(smithScore, 0, 100);
        GamePhase.Instance.SmithPoints += Mathf.RoundToInt(smithScore);

        Debug.Log($"SMITH SCORE = {smithScore}");

        if (audioSource != null)
            audioSource.Stop();

        GamePhase.Instance.SetPhaseQuench();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Smith")) return;
        if (GamePhase.Instance.Smith == 0) return;

        StartSmithing();
    }

    private void StartSmithing()
    {
        Debug.Log("SMITHING STARTED");

        insideSmithZone = true;
        isSmithing = true;

        smithScore = 0f;
        beatTimer = 0f;
        currentBeat = 0;

        if (audioSource != null && smithingBeatTrack != null)
            StartCoroutine(DelayedBeatTrack());
    }

    IEnumerator DelayedBeatTrack()
    {
        yield return new WaitForSeconds(beatTrackDelay);

        audioSource.clip = smithingBeatTrack;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Smith")) return;

        insideSmithZone = false;
        if (isSmithing)
        {
            FinishSmithing();
        }
    }

    public float GetSmithScore()
    {
        return smithScore;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}
