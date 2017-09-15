using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour {

    public static ToolTipManager Instance;

    public GameObject ToolTipPanel;
    public Text MainText;
    public Text BottomText;
    public Toggle ToolTipToggle;

    public void Start()
    {
        Instance = this;

        if (!PlayerPrefs.HasKey("ToolTips"))
        {
            PlayerPrefs.SetInt("ToolTips", 1);
        }
        if(PlayerPrefs.GetInt("ToolTips") == 1)
        {
            ToolTipToggle.isOn = true;
        }
        if (PlayerPrefs.GetInt("ToolTips") == 2)
        {
            ToolTipToggle.isOn = false;
        }
    }

    public void Update()
    {
        if (ToolTipToggle.isOn)
        {
            PlayerPrefs.SetInt("ToolTips", 1);
        }

        if (!ToolTipToggle.isOn)
        {
            PlayerPrefs.SetInt("ToolTips", 2);
          //  Debug.Log("toggle value" + PlayerPrefs.GetInt("ToolTips"));
        }
    }

    public void MouseOver(string mainText, string bottomText = "")
    {
        if (!ToolTipToggle.isOn)
            return;
            // Debug.Log("trying to display");
            ToolTipPanel.SetActive(true);
        MainText.text = mainText;
        BottomText.text = bottomText;
    }

    public void MouseLeavesTTZ()
    {
        ToolTipPanel.SetActive(false);
        ExamineManager.Instance.isExamining = false;
    }


    
}
