using UnityEngine;

public class AnvilAudioTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip whistleClip;

    private bool isPlaying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !isPlaying)
        {
            if (GamePhase.Instance.Smith == 1)
            {
                StartAudio();
            }
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

    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        isPlaying = false;
    }

    public void ResetAudioFlag()
    {
        isPlaying = false;
    }
}
