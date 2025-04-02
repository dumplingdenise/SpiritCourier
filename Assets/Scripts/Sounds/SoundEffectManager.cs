using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = gameObject.AddComponent<AudioSource>(); // Single AudioSource for all sound effects
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject); // Preserve across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate SoundEffectManager
        }
    }

    // Play a specific sound by its name
    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);  // Retrieve clip by name
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);  // Play sound once, no looping
        }
    }

    // Stop all sounds immediately
    public static void Stop()
    {
        audioSource.Stop();  // Stop all playing sounds
    }

    // Check if any sound is currently playing
    public static bool IsPlaying()
    {
        return audioSource.isPlaying;  // Return whether any audio is playing
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });  // Update volume when slider changes
            sfxSlider.value = audioSource.volume; // Set initial slider value to current volume
        }
    }

    // Set the volume of the audio source (affects all sound effects)
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;  // Adjust the audio volume
    }

    // Get the current volume of the audio source
    public float GetVolume()
    {
        return audioSource.volume;  // Return the current volume
    }

    // Handle slider value changes
    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);  // Update the volume of the audio source
    }
}

/*public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
       
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
} */
