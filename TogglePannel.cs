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
    
    
    // Use this for initialization
	void Start ()
    {
        SoundPanel.SetActive(false);
        BuildPanel.SetActive(false);
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
    }

    public void ToggleBuildButtons()
    {
        CloseAllPanelsExcept(BuildPanel);
    }

    public void ToggleSoundButtons()
    {
       CloseAllPanelsExcept(SoundPanel); 
    }


    public void CloseAllPanelsExcept(GameObject panel)
    {
      

        if (panel.activeInHierarchy)
        {
           
            CloseAllPanals();
        }
        else if(!panel.activeInHierarchy)
        {

            CloseAllPanals();
           
            panel.SetActive(true);
           

            
        }
    }

     void CloseAllPanals()
    {
        BuildPanel.SetActive(false);
        SoundPanel.SetActive(false);
        ExamineManager.Instance.StopeExaming();
    }

}
