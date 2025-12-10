using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceLines;
    private int gamestage = 0;

    [Header("References")]
    [SerializeField] private OrderSystem orderSystem;
    [SerializeField] private GamePhase gamePhase;
    private bool line1 = false;
    private bool line2 = false;
    private bool line3 = false;
    private bool line4 = false;
    private bool line5 = false;
    private bool line6 = false;

    private bool hasPlayedForDay = false;

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

        if (gamePhase == null)
        {
            Debug.LogError("GamePhase not assigned in Inspector!");
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
        if (orderSystem == null || gamePhase == null) return;

        if ((orderSystem.GetDayCount() == 0))
        {
            if (line1 == true) return;
            PlayVoiceLine(0);
            line1 = true;

            if (line2 == true) return;
            PlayVoiceLine(1);
            line2 = true;
            
        }


        if ((orderSystem.GetDayCount() == 0) && (gamePhase.Forge == 1))
        {
            if (line3 == true) return;
            PlayVoiceLine(2);
            line3 = true;
        }

        if ((orderSystem.GetDayCount() == 0) && (gamePhase.Smith == 1))
        {
            if (line4 == true) return;
            PlayVoiceLine(3);
            line4 = true;
        }

        if ((orderSystem.GetDayCount() == 0) && (gamePhase.Quench == 1))
        {
            if (line5 == true) return;
            PlayVoiceLine(4);
            line5 = true;
        }

        if ((orderSystem.GetDayCount() == 0) && (gamePhase.Grind == 1))
        {
            if (line6 == true) return;
            PlayVoiceLine(5);
            line6 = true;
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
}