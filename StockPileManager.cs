using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPileManager : MonoBehaviour {

    public Dictionary<Tile, string> StockPileMap;

    public bool StockPileMenueActive = false;

	// Use this for initialization
	void Start () {
        StockPileMap = new Dictionary<Tile, string>();
	}
	
	// Update is called once per frame
	void Update () {

        if (StockPileMenueActive)
            ShowAllStockPiles();
        else
            StopShowingStockPiles();


	}


    public void CreateStockPile()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0)
                continue;
            StockPileMap.Add(t, "");

        }


    }

    public void ShowAllStockPiles()
    {
        if (StockPileMenueActive)
        {
            //TODO::::::::::::::::::::::::::::::::::::::::::::::::::::::
            //when stockpileMenueActive is true highlight all stockpiles in blue
        }

    }

   public void StopShowingStockPiles()
    {
        //TODO:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //despawn blue highlights from view when stockpile menue is closed
    }

    public void RemoveStockPile()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (StockPileMap.ContainsKey(t))
                StockPileMap.Remove(t);

        }
    }

    void CreateStockPileJobs()
    {
        for (int i = 0; i < StockPileMap.Count; i++)
        {
            //TODO:::::::::::::::::::::::::::::::::::::::::::::::::::
            //loop through all the stockpile map and check to see if it has an item on it
            //check to see if that item can hold more on it
            
            //if there is room left either empty or stack not full loop through the worldmanagers loose materials map
            //and see if there is a free item to add to stack or place down if the tile is empty
            //create job to pick up item. then create job to move item to stockpile
            //may need to change the way character looks at jobs for this
        }
    }

}
