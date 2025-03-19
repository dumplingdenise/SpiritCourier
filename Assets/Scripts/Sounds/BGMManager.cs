using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private AudioSource bgmSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private Slider bgmSlider; // reference to the UI slider

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            bgmSource = gameObject.AddComponent<AudioSource>(); // add an audiosource component
            bgmSource.loop = true; // Loop BGM
            bgmSource.playOnAwake = false; //prevent auto playing
            DontDestroyOnLoad(gameObject); // ensure bgm persists across scene
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate BGM instances
            return;
        }
    }

    private void Start()
    {
        PlayBGM(); // start playing bgm

        if (bgmSlider != null) //if a slider is assigned, set up volume control
        {
            bgmSlider.onValueChanged.AddListener(delegate { SetVolume(bgmSlider.value); }); //syn slider with the bgm volume and update volume when slider changes
        }
    }

    public void PlayBGM()
    {
        if (backgroundMusic != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play(); //play bgm
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop(); //stop the music
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = volume; //adjust bgm volume
    }

    public float GetVolume()
    {
        return bgmSource.volume; // return the current volume level
    }
}
