using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    private void OnEnable()
    {
        Instance = this;
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
