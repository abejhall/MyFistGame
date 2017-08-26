using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    public Slider SliderMusic;
    public Slider SliderSoundFX;


    public AudioSource Music;
    public AudioSource SoundEffects;

    public AudioClip pop;
    public AudioClip click;
    public AudioClip uhoh;




    private void Start()
    {
       
        Instance = this;

        //Debug.Log(PlayerPrefs.GetFloat("soundFX") +"player prefs");
        //Debug.Log(SoundEffects.volume+" sFX volume");

        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetFloat("music", SliderMusic.value);
        }
        if (!PlayerPrefs.HasKey("sfx"))
        {
            PlayerPrefs.SetFloat("sfx", SliderSoundFX.value);
        }

       

             SliderSoundFX.value = PlayerPrefs.GetFloat("sfx");
               SliderMusic.value = PlayerPrefs.GetFloat("music");

               SliderMusic.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SliderSoundFX.onValueChanged.AddListener(delegate { ValueChangeCheck2(); });

    }
    public void ValueChangeCheck()
    {
        PlayerPrefs.SetFloat("music", SliderMusic.value);
        PlayerPrefs.SetFloat("soundFx", SliderSoundFX.value);
     
    }

    public void ValueChangeCheck2()
    {
        PlayerPrefs.SetFloat("music", SliderMusic.value);
        PlayerPrefs.SetFloat("sfx", SliderSoundFX.value);
      
    }



    void OnValueChange()
    {
        Debug.Log("oneDisable");
       
    }

   


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
        SoundEffects.volume = SliderSoundFX.value ;
    }
}
