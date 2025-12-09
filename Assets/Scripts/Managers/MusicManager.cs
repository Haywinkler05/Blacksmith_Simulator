using UnityEngine;
using System.Collections;
using System.Linq;

public class MusicCrossfadeManager : MonoBehaviour
{
    [Header("Music Tracks")]
    [SerializeField] private AudioClip[] musicTracks = new AudioClip[4];

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 5f;
    [SerializeField][Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField] private float crossfadeStartOffset = 5f; // Start crossfade X seconds before track ends

    private AudioSource currentSource;
    private AudioSource nextSource;
    private int currentTrackIndex = 0;
    private bool isCrossfading = false;
    private bool hasScheduledCrossfade = false;

    void Start()
    {
        // Create two AudioSource components for crossfading
        currentSource = gameObject.AddComponent<AudioSource>();
        nextSource = gameObject.AddComponent<AudioSource>();

        // Configure AudioSources
        currentSource.loop = false; // Changed to false so we can detect when it ends
        nextSource.loop = false;
        currentSource.playOnAwake = false;
        nextSource.playOnAwake = false;

        // Start playing the first track
        if (musicTracks.Length > 0 && musicTracks[0] != null)
        {
            currentTrackIndex = Random.Range(0, musicTracks.Length);
            currentSource.clip = musicTracks[currentTrackIndex];
            currentSource.volume = masterVolume;
            currentSource.Play();

            Debug.Log($"Started track {currentTrackIndex}: {musicTracks[currentTrackIndex].name}");
        }
    }

    void Update()
    {
        // Update volumes based on master volume
        if (!isCrossfading)
        {
            currentSource.volume = masterVolume;
        }

        // Check if we should start crossfading before the track ends
        if (currentSource.isPlaying && !isCrossfading && !hasScheduledCrossfade)
        {
            float timeRemaining = currentSource.clip.length - currentSource.time;

            if (timeRemaining <= crossfadeStartOffset)
            {
                hasScheduledCrossfade = true;
                CycleToNextTrack();
            }
        }

        // Press Space to cycle to next track (for testing)
        if (Input.GetKeyDown(KeyCode.Space) && !isCrossfading)
        {
            CycleToNextTrack();
        }
    }

    public void CycleToNextTrack()
    {
        if (isCrossfading || musicTracks.Length == 0) return;

        if (musicTracks.Length == 1)
        {
            // Only one track, restart it
            currentSource.time = 0;
            currentSource.Play();
            return;
        }

        // Pick a random track that's NOT the current one
        int nextTrackIndex;
        do
        {
            nextTrackIndex = Random.Range(0, musicTracks.Length);
        } while (nextTrackIndex == currentTrackIndex);

        currentTrackIndex = nextTrackIndex;

        if (musicTracks[currentTrackIndex] != null)
        {
            Debug.Log($"Crossfading to track {currentTrackIndex}: {musicTracks[currentTrackIndex].name}");
            StartCoroutine(CrossfadeToTrack(currentTrackIndex));
        }
    }

    private IEnumerator CrossfadeToTrack(int trackIndex)
    {
        isCrossfading = true;

        // Setup next track
        nextSource.clip = musicTracks[trackIndex];
        nextSource.volume = 0f;
        nextSource.time = 0f; // Make sure it starts from the beginning
        nextSource.Play();

        float elapsed = 0f;
        float startVolume = currentSource.volume;

        // Crossfade
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(startVolume, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, masterVolume, t);

            yield return null;
        }

        // Finalize
        currentSource.Stop();
        currentSource.volume = masterVolume;

        // Swap sources (reusing the same two AudioSource components)
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        isCrossfading = false;
        hasScheduledCrossfade = false; // Reset for next track
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (!isCrossfading)
        {
            currentSource.volume = masterVolume;
        }
    }

    public void NextTrack()
    {
        CycleToNextTrack();
    }
}