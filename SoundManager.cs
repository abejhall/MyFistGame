using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    private void OnEnable()
    {
        Debug.Log("oneEnable");
        Instance = this;
        if(!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetFloat("music", SliderMusic.value);
        }
        if (!PlayerPrefs.HasKey("soundFX"))
        {
            PlayerPrefs.SetFloat("soundFX", SliderSoundEffects.value);
        }

        SoundEffects.volume = PlayerPrefs.GetFloat("soundFX");
        Music.volume = PlayerPrefs.GetFloat("music");

    }

    private void OnDisable()
    {
        Debug.Log("oneDisable");
        PlayerPrefs.SetFloat("music", SliderMusic.value);
        PlayerPrefs.SetFloat("soundFx", SliderSoundEffects.value);
    }

    public Slider SliderMusic;
    public Slider SliderSoundEffects;


    public  AudioSource Music;
    public AudioSource SoundEffects;

    public  AudioClip pop;
    public  AudioClip click;
    public AudioClip uhoh;



    public  void PlayPopSound()
    {
        SoundEffects.PlayOneShot(pop);
    }

    public void PlayUhOhSound()
    {
        SoundEffects.PlayOneShot(uhoh);
    }

    void Update()
    {
        Music.volume = SliderMusic.value;
        SoundEffects.volume = SliderSoundEffects.value ;
    }
}
