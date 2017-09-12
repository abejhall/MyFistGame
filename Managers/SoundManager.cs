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

    public AudioClip Mining;

    public AudioClip Chopping;

    public List<AudioClip> clips;
    Dictionary<string, AudioClip> AudioClipMap;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AudioClipMap = new Dictionary<string, AudioClip>();
        
        //clips = new List<AudioClip>();
        PopulateAduioClipMap();

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

    void PopulateAduioClipMap()
    {
        for (int i = 0; i < clips.Count; i++)
        {
            AudioClipMap.Add(clips[i].name,clips[i]);
          
        }

        //Debug.Log("check to see if map populates" + AudioClipMap["Chopping"]);
    }

    void OnValueChange()
    {
        Debug.Log("oneDisable");
       
    }

   
    public void PlaySound(string sound, AudioSource aud)
    {
   
        AudioClip tmpclip;
        if (AudioClipMap.ContainsKey(sound))
        {
            tmpclip = AudioClipMap[sound];
        }
            else
        {
             Debug.LogWarning("could not find a sound with the name: " + sound + " in the audioManager ");
             return;
        }

        aud.PlayOneShot(tmpclip);
       
        
    }

    public  void PlayPopSound()
    {
        SoundEffects.PlayOneShot(pop);
    }

    public void PlayUhOhSound()
    {
        SoundEffects.PlayOneShot(uhoh);
    }

    public void PlayChoppingSound()
    {
        SoundEffects.PlayOneShot(Chopping);
    }
    public void PlayMiningSound()
    {
        SoundEffects.PlayOneShot(Mining);
    }

    void Update()
    {
        Music.volume = SliderMusic.value;
        SoundEffects.volume = SliderSoundFX.value ;
    }
}
