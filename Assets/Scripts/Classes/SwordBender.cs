using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBender : MonoBehaviour
{
    private Quaternion[] originalRotations;

    [Header("Score Thresholds (0 - 100)")]
    public int perfectMinScore = 85;   // 85–100 → Perfect
    public int goodMinScore = 70;      // 70–84 → Good
    public int okMinScore = 40;      // 40–69 → OK
    // Anything below 40 → Bad

    [Header("Bone Bending Settings")]
    public Transform[] bladeBones;     // Assign blade bones in order from handle → tip

    [Header("Good Sword Settings")]
    public float goodMaxBend = 4f;         // Max degrees per bone
    public int goodMinBends = 1;           // # of bend bones
    public int goodMaxBends = 2;

    [Header("OK Sword Settings")]
    public float okMaxBend = 10f;
    public int okMinBends = 3;
    public int okMaxBends = 5;

    [Header("Bad Sword Settings")]
    public float badMaxBend = 25f;
    public bool allowCrazyDirections = true;

    //testing script
    [Range(0, 100)]
    public int testScore = 50;

    [ContextMenu("Apply Test Score")]
    public void ApplyTestScore()
    {
        ApplyBendsFromScore(testScore);
    }

    private void ResetBones()
    {
        foreach (var bone in bladeBones)
            bone.localRotation = Quaternion.identity;
    }

    void Start()
    {
        // Save original “straight” rotations
        originalRotations = new Quaternion[bladeBones.Length];
        for (int i = 0; i < bladeBones.Length; i++)
            originalRotations[i] = bladeBones[i].localRotation;
    }


    /// <summary>
    /// Call this from your minigame when the score is known.
    /// </summary>
    public void ApplyBendsFromScore(int score)
    {
        // Always reset the sword first
        ResetBones();

        if (score >= perfectMinScore)
        {
            Debug.Log("Perfect → Reset to straight sword.");
            return;
        }
        else if (score >= goodMinScore)
        {
            ApplyGoodBends(score);
        }
        else if (score >= okMinScore)
        {
            ApplyOKBends(score);
        }
        else
        {
            ApplyBadBends(score);
        }
    }


    // ------------------- GOOD -------------------
    private void ApplyGoodBends(int score)
    {
        int numBends = Random.Range(goodMinBends, goodMaxBends + 1);
        float maxAngle = Mathf.Lerp(goodMaxBend * 0.5f, goodMaxBend, 1 - (score - goodMinScore) / 15f);

        BendRandomBones(numBends, maxAngle, straightSword: true);
    }

    // ------------------- OK ----------------------
    private void ApplyOKBends(int score)
    {
        int numBends = Random.Range(okMinBends, okMaxBends + 1);
        float maxAngle = Mathf.Lerp(okMaxBend * 0.5f, okMaxBend, 1 - (score - okMinScore) / 30f);

        BendRandomBones(numBends, maxAngle, straightSword: true);
    }

    // ------------------- BAD ---------------------
    private void ApplyBadBends(int score)
    {
        // Completely ignore straightness rules
        BendRandomBones(
            numBones: Random.Range(2, bladeBones.Length),
            maxAngle: badMaxBend,
            straightSword: !allowCrazyDirections
        );
    }

    // ------------------------------------------------------
    // Generic bone bending function
    // ------------------------------------------------------
    private void BendRandomBones(int numBones, float maxAngle, bool straightSword)
    {
        for (int i = 0; i < numBones; i++)
        {
            int index = Random.Range(0, bladeBones.Length);
            float angle = Random.Range(-maxAngle, maxAngle);

            Vector3 bendAxis = straightSword
                ? bladeBones[index].forward    // ← rotate around Z axis
                : Random.onUnitSphere;         // bad swords can bend any direction

            bladeBones[index].localRotation *= Quaternion.AngleAxis(angle, bendAxis);
        }
    }


}
