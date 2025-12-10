using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilAudioTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;   // Attach the AudioSource from the anvil
    public AudioClip whistleClip;     // The 30-second smithing tune

    private bool isPlaying = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if a sword enters
        if (other.CompareTag("Sword") && !isPlaying)
        {
            StartAudio();
        }
    }

    private void StartAudio()
    {
        if (audioSource != null && whistleClip != null)
        {
            audioSource.clip = whistleClip;
            audioSource.Play();
            isPlaying = true;
            Debug.Log("Anvil: Smithing audio started!");
        }
    }

    // Optional: stop audio manually if needed
    public void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
