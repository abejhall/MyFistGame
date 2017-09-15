using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePannel : MonoBehaviour {

    public GameObject pannel;
    RectTransform rec;
    public bool toggleBuildButtons;
    public bool toggleSettingsButtons;
    public bool toggleStockPileButton;
    public bool toggleCharacterMenueButtons;

    public GameObject SoundPanel;
    public GameObject BuildPanel;
    public GameObject StockPilePanel;
    public GameObject CharacterMenue;

    public Animator BuildAnim;
    public Animator SettingsAnim;
    public Animator STOCKAnim;
    public Animator CharterAnim;

    public float fadeTime = .5f;







    //Hot Keys allows for a remapping later by the user in
    // a furture settings menue

    public KeyCode BuildMenueHK = KeyCode.B;
    public KeyCode CharacterMenueHK = KeyCode.C;
    public KeyCode StockPileHK = KeyCode.N;
    public KeyCode SettingsHK = KeyCode.V;
    public KeyCode ExamineHK = KeyCode.M;




    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            //ToggleBuildButtons();
        }

        HotKeys();
    }


    void HotKeys()
    {
        if (Input.GetKeyDown(BuildMenueHK))
        {
            ToggleBuildButtons();
        }

        if (Input.GetKeyDown(CharacterMenueHK))
        {
            ToggleCharacterButtons();
        }

        if (Input.GetKeyDown(StockPileHK))
        {
            ToggleStockPileButton();
        }

        if (Input.GetKeyDown(SettingsHK))
        {
            ToggleSettingsButtons();
        }

        if (Input.GetKeyDown(ExamineHK))
        {
            ExamineManager.Instance.IsExamining();
        }
        
    }

    public void ToggleStockPileButton ()
    {
        toggleStockPileButton = !toggleStockPileButton;



        //if build Panel is still open close it before opening stockpile panel
        if (toggleCharacterMenueButtons)
        {
            toggleStockPileButton = !toggleStockPileButton;
            CharterAnim.SetTrigger("close");
            toggleCharacterMenueButtons = false;
            Invoke("ToggleStockPileButton", fadeTime);
            return;
        }


        //if build Panel is still open close it before opening stockpile panel
        if (toggleBuildButtons)
        {
            toggleStockPileButton = !toggleStockPileButton;
            BuildAnim.SetTrigger("close");
            toggleBuildButtons = false;
            Invoke("ToggleStockPileButton", fadeTime);
            return;
        }



        //if Settings Panel is still open close it before opening stockpile panel
        if (toggleSettingsButtons)
        {
            toggleStockPileButton = !toggleStockPileButton;
            SettingsAnim.SetTrigger("Close");
            toggleSettingsButtons = false;
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
        if (toggleCharacterMenueButtons)
        {
            toggleBuildButtons = !toggleBuildButtons;
            CharterAnim.SetTrigger("close");
            toggleCharacterMenueButtons = false;
            Invoke("ToggleBuildButtons", fadeTime);
            return;
        }


        

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
        if (toggleSettingsButtons)
        {
            toggleBuildButtons = !toggleBuildButtons;
            SettingsAnim.SetTrigger("Close");
            toggleSettingsButtons = false;
            Invoke("ToggleBuildButtons", fadeTime);
            return;
            
        }


        if (toggleBuildButtons)
        {
            BuildAnim.SetTrigger("open");
            return;
        }


        if (!toggleBuildButtons)
        {
            BuildAnim.SetTrigger("close");
            return;
        }
    }
            

    public void ToggleSettingsButtons()
    {
        toggleSettingsButtons = !toggleSettingsButtons;

        if (toggleCharacterMenueButtons)
        {
            toggleSettingsButtons = !toggleSettingsButtons;
            CharterAnim.SetTrigger("close");
            toggleCharacterMenueButtons = false;
            Invoke("ToggleSettingsButtons", fadeTime);
            return;
        }



        //if stockpile Panel is still open close it before opening settings panel
        if (toggleStockPileButton)
        {
            toggleSettingsButtons = !toggleSettingsButtons;
            STOCKAnim.SetTrigger("close");
            toggleStockPileButton = false;
            Invoke("ToggleSettingsButtons", fadeTime);
            return;
        }




        if (toggleBuildButtons)
        {
            toggleSettingsButtons = !toggleSettingsButtons;
            BuildAnim.SetTrigger("close");
            toggleBuildButtons = false;
            Invoke("ToggleSettingsButtons", fadeTime);
            return;
        }

        if (toggleSettingsButtons)
        {
            SettingsAnim.SetTrigger("Open");
            return;
        }
        if (!toggleSettingsButtons)
        {
            SettingsAnim.SetTrigger("Close");
            return;
        }
    }

    public void ToggleCharacterButtons()
    {
        toggleCharacterMenueButtons = !toggleCharacterMenueButtons;

        if (toggleCharacterMenueButtons)
        DisplayCharacterUI.Instance.GetCharacterUIs();

       

        if (toggleSettingsButtons)
        {
            toggleCharacterMenueButtons = !toggleCharacterMenueButtons;
            SettingsAnim.SetTrigger("Close");
            toggleSettingsButtons = false;
            Invoke("ToggleCharacterButtons", fadeTime);
            return;
        }



        //if stockpile Panel is still open close it before opening settings panel
        if (toggleStockPileButton)
        {
            toggleCharacterMenueButtons = !toggleCharacterMenueButtons;
            STOCKAnim.SetTrigger("close");
            toggleStockPileButton = false;
            Invoke("ToggleCharacterButtons", fadeTime);
            return;
        }




        if (toggleBuildButtons)
        {
            toggleCharacterMenueButtons = !toggleCharacterMenueButtons;
            BuildAnim.SetTrigger("close");
            toggleBuildButtons = false;
            Invoke("ToggleCharacterButtons", fadeTime);
            return;
        }

        if (toggleCharacterMenueButtons)
        {
            CharterAnim.SetTrigger("open");
            return;
        }
        if (!toggleCharacterMenueButtons)
        {
            CharterAnim.SetTrigger("close");
            return;
        }
    }
   

}
