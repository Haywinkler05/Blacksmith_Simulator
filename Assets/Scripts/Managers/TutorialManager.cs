using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceLines;
    private int gamestage = 0;

    [Header("References")]
    [SerializeField] private OrderSystem orderSystem;
    [SerializeField] private GamePhase gameState;

    // Track which voice lines have already been played
    private bool hasPlayedVoiceLine = false;

    void Start()
    {
        // Get AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Check if references are assigned
        if (orderSystem == null)
        {
            Debug.LogError("OrderSystem not assigned in Inspector!");
        }
    }

    void Update()
    {
        // Check conditions and play voice lines
        CheckAndPlayVoiceLines();
    }

    public void CheckAndPlayVoiceLines()
    {
        // Make sure we have references to the scripts
        if (orderSystem == null) return;

        // Only check if we haven't played a voice line yet
        if (hasPlayedVoiceLine) return;

        if (orderSystem.GetDayCount() == 0)
        {
            if (gameState.Forge == 1)
            {
                PlayVoiceLine(0);
                hasPlayedVoiceLine = true;
            }
            else if (gameState.Smith == 1)
            {
                PlayVoiceLine(1);
                hasPlayedVoiceLine = true;
            }
            else if (gameState.Quench == 1)
            {
                PlayVoiceLine(2);
                hasPlayedVoiceLine = true;
            }
            else if (gameState.Grind == 1)
            {
                PlayVoiceLine(3);
                hasPlayedVoiceLine = true;
            }
        }
    }

    private void PlayVoiceLine(int lineIndex)
    {
        if (voiceLines == null || lineIndex >= voiceLines.Length)
        {
            Debug.LogWarning($"Voice line {lineIndex} is out of range or array is null!");
            return;
        }

        if (audioSource != null && voiceLines[lineIndex] != null)
        {
            audioSource.clip = voiceLines[lineIndex];
            audioSource.Play();
            Debug.Log($"Playing voice line {lineIndex}");
        }
        else
        {
            Debug.LogError("AudioSource or voice line clip is missing!");
        }
    }

    // Call this method when you want to reset and allow voice lines to play again
    // For example, when moving to a new tutorial stage or new day
    public void ResetVoiceLineFlag()
    {
        hasPlayedVoiceLine = false;
    }
}