using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithingStage : MonoBehaviour
{
    private StateMachine stateMachine;

    [Header("Audio")]
    public AudioSource beatAudio;
    public float bpm = 100f;

    [Header("Hit Settings")]
    public float perfectWindow = 0.05f;
    public float goodWindow = 0.12f;
    public float okWindow = 0.20f;

    [Header("Scoring")]
    public int hitsRequired = 10;
    private int hitsDone = 0;

    private float beatInterval;
    private float songStartTime;

    private List<float> hitScores = new List<float>();

    void Awake()
    {
        stateMachine = FindObjectOfType<StateMachine>();
        enabled = false;
    }

    public void StartStage()
    {
        Debug.Log("Smithing Stage Started!");
        beatInterval = 60f / bpm;

        beatAudio.time = 0f;
        beatAudio.Play();

        songStartTime = Time.time;
        hitsDone = 0;
        hitScores.Clear();
    }

    void Update()
    {
        // TEMP INPUT FOR TESTING (player hit)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegisterHit();
        }

        if (hitsDone >= hitsRequired)
        {
            EndStage();
        }
    }

    void RegisterHit()
    {
        float currentTime = Time.time - songStartTime;

        // How far are we from the nearest beat?
        float beatNumber = Mathf.Round(currentTime / beatInterval);
        float nearestBeatTime = beatNumber * beatInterval;
        float deviation = Mathf.Abs(currentTime - nearestBeatTime);

        float score = EvaluateHit(deviation);
        hitScores.Add(score);
        hitsDone++;

        Debug.Log($"Hit {hitsDone}/{hitsRequired} | deviation={deviation} | score={score}");
    }

    float EvaluateHit(float deviation)
    {
        if (deviation <= perfectWindow)
            return 1f;          // Perfect (100 points)

        if (deviation <= goodWindow)
            return 0.75f;       // Good  (75 points)

        if (deviation <= okWindow)
            return 0.4f;        // Ok    (40 points)

        return 0f;              // Miss  (0 points)
    }

    void EndStage()
    {
        // Calculate final score (0–100)
        float avg = 0f;
        foreach (float s in hitScores)
            avg += s;

        avg /= hitScores.Count;
        int finalScore = Mathf.RoundToInt(avg * 100);

        Debug.Log($"Smithing Stage Finished. Final Score: {finalScore}");

        // send score to state machine
        stateMachine.CompleteCurrentStage(finalScore);

        enabled = false; // Disable this stage’s Update()
    }
}
