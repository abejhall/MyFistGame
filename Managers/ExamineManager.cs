using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineManager : MonoBehaviour {

    public static ExamineManager Instance;

    bool toggle = false;
    string TextToShow = "null";
   public bool isExamining = false;

    string StockPileInfoToBeDisplayed;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {



        if (isExamining)
        {

            Tile t = SelectionManager.Instance.GetTileUnderMouse();
            StockpileInfo(t);

              TextToShow = "Type: " + t.type + " " + "MovementSpeed: " +
              t.MovementSpeedAdjustment+"  X:"+t.x + "Y:" + t.y+StockPileInfoToBeDisplayed+
              "RoomDevider:"+t.RoomDevider+"RoomFloor:"+t.RoomFloor;

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


    public void StockpileInfo(Tile t)
    {
        if (t.type == "stockpile")
        {
            string wood;
            string food;
            string medicine;
            string weapons;
            string stone;
            StockPile sp = WorldManager.Instance.TileToGameObjectMap[t].GetComponent<StockPile>();

            wood = sp.wood ? "wood, " : "";
            food = sp.food ? "Food, " : "";
            medicine = sp.medicine ? "Medicine, " : "";
            weapons = sp.weapons ? "Weapons, " : "";
            stone = sp.stone ? "Stone, " : "";

            StockPileInfoToBeDisplayed = "StockPile Location for: " + wood + food + medicine + weapons + stone;
        }
        else StockPileInfoToBeDisplayed = "";
    }



    public void StopExam()
    {
        //isExamining = false;
        ToolTipManager.Instance.MouseLeavesTTZ();
     //   Debug.Log("stop exame");
    }
}
