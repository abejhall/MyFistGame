using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour {

    public static ToolTipManager Instance;

    public GameObject ToolTipPanel;
    public Text MainText;
    public Text BottomText;

    public void Start()
    {
        Instance = this;
    }


    public void MouseOver(string mainText, string bottomText = "")
    {
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
