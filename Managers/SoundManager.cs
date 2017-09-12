using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class SoundManager : MonoBehaviour {

    

    public Slider SliderMusic;
    public Slider SliderSoundFX;


    public AudioSource Music;
    public AudioSource SoundEffects;

   
    //this is the list of clips added in the inspector to be used in the game
    public List<AudioClip> clips;

    //this private dic is to keep track of the string names so they can be called by the string from anywhere in the game.
    Dictionary<string, AudioClip> AudioClipMap;


    #region singleton
    public static SoundManager Instance;
    private void Awake()
    {
        Instance = this;
    }
#endregion

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

       
            //set the audio settings to what they were in last played game.
             SliderSoundFX.value = PlayerPrefs.GetFloat("sfx");
             SliderMusic.value = PlayerPrefs.GetFloat("music");

        //add callbacks for when the sliders change to update the playerprefs
        SliderMusic.onValueChanged.AddListener(delegate { ValueChangeCheckMusic(); });
        SliderSoundFX.onValueChanged.AddListener(delegate { ValueChangeCheckSFX(); });

    }



    public void ValueChangeCheckMusic()
    {
        PlayerPrefs.SetFloat("music", SliderMusic.value);
        PlayerPrefs.SetFloat("soundFx", SliderSoundFX.value);
     
    }

    //check if the value of the slider has changed and save them in the playerprefs if they have
    public void ValueChangeCheckSFX()
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

  //  void OnValueChange()
  //  {
  //      Debug.Log("oneDisable");
       
  //  }

   
        //play a sound at a specific audio source
    public void PlaySound(string sound, AudioSource aud)
    {
   
        AudioClip tmpclip;
        if (AudioClipMap.ContainsKey(sound))
        {
            tmpclip = AudioClipMap[sound];
        }
            else
        {
            Debug.LogError("A request to play a sound named:" + sound +
                " was given but not found in the list of sound names.  Please check to see that this sound is spelled correctly!");
            return;
        }

        aud.PlayOneShot(tmpclip);
       
        
    }

    //play a sound at the default sound effects audiosource
    public void PlaySound(string sound)
    {

        AudioClip tmpclip;
        if (AudioClipMap.ContainsKey(sound))
        {
            tmpclip = AudioClipMap[sound];
        }
        else
        {
            Debug.LogError("A request to play a sound named:"+sound+
                " was given but not found in the list of sound names.  Please check to see that this sound is spelled correctly!");
            return;
        }

        SoundEffects.PlayOneShot(tmpclip);


    }


    void Update()
    {
        Music.volume = SliderMusic.value;
        SoundEffects.volume = SliderSoundFX.value ;
    }
}
