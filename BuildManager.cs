using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    public static BuildManager Instance;



    public Sprite[] walls;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	





    public void BuildTilledLand()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f  )
                continue;
            t.type = "dirt";
            JobManager.Instance.CreateJob(t, WorldManager.Instance.blimish, 0f, false, .5f);
            
        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void PlantFlower()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;
            t.type = "plant";
            JobManager.Instance.CreateJob(t, WorldManager.Instance.plant, 0f, false, .5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void DestoryTile()
    {
      //  Debug.Log("Destroy tile");

        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 1f)
                continue;

            t.MovementSpeedAdjustment = 1f;
            t.type = "grass";
            JobManager.Instance.CreateJob(t, WorldManager.Instance.grass, 1f, false, .5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }


    public void BuildWall ()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;

            Sprite wall = walls[0];
            t.type = "wall";
            JobManager.Instance.CreateJob(t, wall, 0f,true, .5f);


        }
        SelectionManager.Instance.DestroyHighlight();
    }



    public void CheckNeighbors(Tile t, bool fc)
    {
        if (t.type != "wall")
            return;
        
        string wall = "wall_";


        Tile EN = WorldManager.Instance.GetTileAT(t.x +1, t.y);
        if(EN.type == "wall")
        {
            wall = wall + "e";
            if(fc)
            CheckNeighbors(EN,false);
        }
        Tile SN = WorldManager.Instance.GetTileAT(t.x , t.y - 1); //(t.x + 1, t.y - 1)
        if (SN.type == "wall")
        {
            wall = wall + "s";
            if (fc)
                CheckNeighbors(SN, false);
        }
        Tile WN = WorldManager.Instance.GetTileAT(t.x -1, t.y);
        if (WN.type == "wall")
        {
            wall = wall + "w";
            if (fc)
                CheckNeighbors(WN, false);
        }
        Tile NN = WorldManager.Instance.GetTileAT(t.x , t.y +1);
        if (NN.type == "wall")
        {
            wall = wall + "n";
            if (fc)
                CheckNeighbors(NN, false);
        }
           


        //Tile BRN = WorldManager.Instance.GetTileAT(t.x + 1, t.y-1);
        // Tile BLN = WorldManager.Instance.GetTileAT(t.x - 1, t.y -1);
        // Tile TRN = WorldManager.Instance.GetTileAT(t.x + 1, t.y +1);
        // Tile TLN = WorldManager.Instance.GetTileAT(t.x + 1, t.y -1);

        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        foreach(Sprite s in walls)
        {
            if (s.name == wall)
                sr.sprite = s;
            Debug.Log("changing neighbor"+wall);
        }
        



    }



}
