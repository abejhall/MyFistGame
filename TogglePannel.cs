using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePannel : MonoBehaviour {

    public GameObject pannel;
    RectTransform rec;
    public bool toggleBuildButtons;
    public bool toggleSoundButtons;

    public GameObject SoundPanel;
    public GameObject BuildPanel;

    public Animator anim;
    public Animator SPAnim;

    public float fadeTime = .5f;
    
    
    // Use this for initialization
	void Start ()
    {
        

     
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
    }

    public void ToggleBuildButtons()
    {
        

        toggleBuildButtons = !toggleBuildButtons;

        if (toggleSoundButtons)
        {
            toggleBuildButtons = !toggleBuildButtons;
            SPAnim.SetTrigger("Close");
            toggleSoundButtons = false;
            Invoke("ToggleBuildButtons", fadeTime);
            return;
            
        }


        if (toggleBuildButtons)
        {
            anim.SetTrigger("OpenPanel");
            return;
        }


        if (!toggleBuildButtons)
        {
            anim.SetTrigger("ClosePanel");
            return;
        }
    }
            

    public void ToggleSoundButtons()
    {
        toggleSoundButtons = !toggleSoundButtons;
       

        if (toggleBuildButtons)
        {
            toggleSoundButtons = !toggleSoundButtons;
            anim.SetTrigger("ClosePanel");
            toggleBuildButtons = false;
            Invoke("ToggleSoundButtons",fadeTime);
            return;
        }

        if (toggleSoundButtons)
        {
            SPAnim.SetTrigger("Open");
            return;
        }
        if (!toggleSoundButtons)
        {
            SPAnim.SetTrigger("Close");
            return;
        }
    }


   

   

}
