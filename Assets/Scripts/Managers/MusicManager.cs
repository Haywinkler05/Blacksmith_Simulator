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

    private AudioSource currentSource;
    private AudioSource nextSource;
    private int currentTrackIndex = 0;
    private bool isCrossfading = false;

    void Start()
    {
        // Create two AudioSource components for crossfading
        currentSource = gameObject.AddComponent<AudioSource>();
        nextSource = gameObject.AddComponent<AudioSource>();

        // Configure AudioSources
        currentSource.loop = true;
        nextSource.loop = true;
        currentSource.playOnAwake = false;
        nextSource.playOnAwake = false;

        // Start playing the first track
        if (musicTracks.Length > 0 && musicTracks[0] != null)
        {
            currentTrackIndex = Random.Range(0, musicTracks.Length); // SET THE INDEX!
            currentSource.clip = musicTracks[currentTrackIndex];
            currentSource.volume = masterVolume;
            currentSource.Play();
        }
    }

    void Update()
    {
        // Update volumes based on master volume
        if (!isCrossfading)
        {
            currentSource.volume = masterVolume;
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

        // Move to next track index
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;

        if (musicTracks[currentTrackIndex] != null)
        {
            StartCoroutine(CrossfadeToTrack(currentTrackIndex));
        }
    }

    private IEnumerator CrossfadeToTrack(int trackIndex)
    {
        isCrossfading = true;

        // Setup next track
        nextSource.clip = musicTracks[trackIndex];
        nextSource.volume = 0f;
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

        // Swap sources
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        isCrossfading = false;
    }

    // Public method to set master volume
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);

        if (!isCrossfading)
        {
            currentSource.volume = masterVolume;
        }
    }

    // Public method to trigger track change from UI or other scripts
    public void NextTrack()
    {
        CycleToNextTrack();
    }
}