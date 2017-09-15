using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustMyAudio : MonoBehaviour {
    Camera cam;
    AudioSource aud;
	// Use this for initialization
	void Start () {
        aud = GetComponent<AudioSource>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        //aud.volume = SoundManager.Instance.SliderSoundFX.value;

        if(cam.orthographicSize >= 14f)
        {
            aud.volume = SoundManager.Instance.SliderSoundFX.value - 1;
        }
        else if(cam.orthographicSize >6f && cam.orthographicSize < 10f)
        {
            aud.volume = SoundManager.Instance.SliderSoundFX.value *.75f;
        }
        else if (cam.orthographicSize > 10f && cam.orthographicSize < 12f)
        {
            aud.volume = SoundManager.Instance.SliderSoundFX.value *.5f;
        }
        else if (cam.orthographicSize > 12f && cam.orthographicSize < 14f)
        {
            aud.volume = SoundManager.Instance.SliderSoundFX.value * .25f;
        }
        else
            aud.volume = SoundManager.Instance.SliderSoundFX.value;
    }
}
