using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePannel : MonoBehaviour {

    public GameObject pannel;
    RectTransform rec;
    public bool toggleBuildButtons;
    public bool toggleSoundButtons;
    public bool toggleStockPileButton;

    public GameObject SoundPanel;
    public GameObject BuildPanel;
    public GameObject StockPilePanel;

    public Animator BuildAnim;
    public Animator SettingsAnim;
    public Animator STOCKAnim;

    public float fadeTime = .5f;
    
    public void ToggleStockPileButton ()
    {
        toggleStockPileButton = !toggleStockPileButton;


        //if build Panel is still open close it before opening stockpile panel
        if (toggleBuildButtons)
        {
            toggleStockPileButton = !toggleStockPileButton;
            BuildAnim.SetTrigger("ClosePanel");
            toggleBuildButtons = false;
            Invoke("ToggleStockPileButton", fadeTime);
            return;
        }



        //if Settings Panel is still open close it before opening stockpile panel
        if (toggleSoundButtons)
        {
            toggleStockPileButton = !toggleStockPileButton;
            SettingsAnim.SetTrigger("Close");
            toggleSoundButtons = false;
            Invoke("ToggleStockPileButton", fadeTime);
            return;

        }


        if (toggleStockPileButton)
        {
            STOCKAnim.SetTrigger("open");
            return;
        }
        if (!toggleStockPileButton)
        {
            STOCKAnim.SetTrigger("close");
            return;
        }
    }
    

    public void ToggleBuildButtons()
    {
        

        toggleBuildButtons = !toggleBuildButtons;

        //if stockpile Panel is still open close it before opening stockpile panel
        if (toggleStockPileButton)
        {
            toggleBuildButtons = !toggleBuildButtons;
            STOCKAnim.SetTrigger("close");
            toggleStockPileButton = false;
            Invoke("ToggleBuildButtons", fadeTime);
            return;
        }




        //if Settings Panel is still open close it before opening build panel
        if (toggleSoundButtons)
        {
            toggleBuildButtons = !toggleBuildButtons;
            SettingsAnim.SetTrigger("Close");
            toggleSoundButtons = false;
            Invoke("ToggleBuildButtons", fadeTime);
            return;
            
        }


        if (toggleBuildButtons)
        {
            BuildAnim.SetTrigger("OpenPanel");
            return;
        }


        if (!toggleBuildButtons)
        {
            BuildAnim.SetTrigger("ClosePanel");
            return;
        }
    }
            

    public void ToggleSoundButtons()
    {
        toggleSoundButtons = !toggleSoundButtons;


        //if stockpile Panel is still open close it before opening settings panel
        if (toggleStockPileButton)
        {
            toggleSoundButtons = !toggleSoundButtons;
            STOCKAnim.SetTrigger("close");
            toggleStockPileButton = false;
            Invoke("ToggleSoundButtons", fadeTime);
            return;
        }




        if (toggleBuildButtons)
        {
            toggleSoundButtons = !toggleSoundButtons;
            BuildAnim.SetTrigger("ClosePanel");
            toggleBuildButtons = false;
            Invoke("ToggleSoundButtons",fadeTime);
            return;
        }

        if (toggleSoundButtons)
        {
            SettingsAnim.SetTrigger("Open");
            return;
        }
        if (!toggleSoundButtons)
        {
            SettingsAnim.SetTrigger("Close");
            return;
        }
    }

    

   

}
