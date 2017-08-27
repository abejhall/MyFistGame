using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineManager : MonoBehaviour {

    public static ExamineManager Instance;

    bool toggle = false;
    string TextToShow = "null";
   public bool isExamining = false;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {



        if (isExamining)
        {

            Tile t = SelectionManager.Instance.GetTileUnderMouse();
            TextToShow = "Type: " + t.type + " " + "MovementSpeed: " + t.MovementSpeedAdjustment+"  X:"+t.x + "Y:" + t.y;
            ToolTipManager.Instance.MouseOver(TextToShow);
        }

        

	}

    public void IsExamining()
    {
        toggle = !toggle;
        isExamining = true;
        if (!toggle)
            StopExam();
    }

    public void StopExam()
    {
        //isExamining = false;
        ToolTipManager.Instance.MouseLeavesTTZ();
     //   Debug.Log("stop exame");
    }
}
