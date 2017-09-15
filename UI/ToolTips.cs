using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTips : MonoBehaviour {

    public void DisplayPickFlowersTT()
    {
        ToolTipManager.Instance.MouseOver("Pick flower peddels to be used for food.", "Flower Peddels can be hauled away to be stored for food or destroyed!");
    }

    public void DisplayChopTreesTT()
    {
        ToolTipManager.Instance.MouseOver("Chop downs trees into logs.", "Logs can be hauled for building supplys or destroyed!");
    }

    public void DisplayMineRocksTT()
    {
        ToolTipManager.Instance.MouseOver("Mine Large rocks into smaller more managable rocks.", "Smaller rocks can be hauled for building supplys or destroyed!");
    }

    public void DisplayPlantFlowerTT()
    {
        ToolTipManager.Instance.MouseOver("Plant a flower to be used for food.", "Can only be planted on tilled land!");
    }


    public void DisplayBuildFloorTT()
    {
        ToolTipManager.Instance.MouseOver("Buid a floor that walls and furniture can be placed on.", "Can only be built on Dirt!");
    }

    public void DisplayBuildDoorTT()
    {
        ToolTipManager.Instance.MouseOver("Build a door to protect your homes!", "Must have 2 walls to connect to!");
    }

    public void DisplayDestroyTileTT()
    {
        ToolTipManager.Instance.MouseOver("Destroy anything you have built!", "Will return that area to grass!");
    }


    public void DisplayBuildWallTT()
    {
        ToolTipManager.Instance.MouseOver("Build walls and create a home!", "Walls can only be placed on floors!");
    }


    public void DisplayCancelJobsTT()
    {
        ToolTipManager.Instance.MouseOver("Cancel all yellow highlighted jobs", "Jobs already started will not be canceled!");
    }

}
