using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    // Dictionary to track when each sound was last played, with MinSoundInterval applied per sound clip
    private Dictionary<AudioClip, float> lastPlayTimes = new Dictionary<AudioClip, float>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Check if MinSoundInterval has passed for this specific clip
        if (lastPlayTimes.TryGetValue(audioClip, out float lastPlayTime))
        {
            float timeSinceLastPlay = Time.time - lastPlayTime;
            if (timeSinceLastPlay < 0.05f) // Short interval to prevent accidental double triggers
            {
                return;
            }
        }

        // Create a new instance of AudioSource for this play
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Update last play time for this specific clip
        lastPlayTimes[audioClip] = Time.time;

        // Destroy the audio source after the clip finishes playing
        Destroy(audioSource.gameObject, audioClip.length);
    }
}
