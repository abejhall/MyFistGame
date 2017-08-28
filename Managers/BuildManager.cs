using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    public static BuildManager Instance;

    public GameObject Vdoor;
    public GameObject Hdoor;

    public Sprite[] walls;

    public Sprite[] doors;

    GrowthManager gm;

	// Use this for initialization
	void Start () {
        Instance = this;

	}
	
    public void CancelJobsAtPoint()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            JobManager.Instance.CancelSelectedJobs(t);
        }
            
    }


    public void DisplayCancelJobsTT()
    {
        ToolTipManager.Instance.MouseOver("Cancel all yellow highlighted jobs", "Jobs already started will not be canceled!");
    }

    public void BuildTilledLand()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.type != "grass")
                continue;
            if (t.MovementSpeedAdjustment == 0f  )
                continue;
            // t.type = "dirt";

            //WorldManager.Instance.Dirt
            JobManager.Instance.CreateJob(t,SpriteManager.Instance.GS("Dirt") ,"dirt", 1f, false, .5f);
            
        }
        SelectionManager.Instance.DestroyHighlight();

    }

            public void DisplayTillLandTT()
    {
        ToolTipManager.Instance.MouseOver("This will till the ground for planting.", "Can only be used on grass!");
    }


    public void PlantFlower()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;
            if (t.type != "dirt")
                continue;


           // t.type = "plant";
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("Plant_0"),"plant", 1f, false, .5f);

            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            gm = go.AddComponent<GrowthManager>();
            gm.t = t;
            gm.timer = 50f;
            gm.stage1 = SpriteManager.Instance.GS("Plant_0");
            gm.stage2 = SpriteManager.Instance.GS("Plant_1");
            gm.stage3 = SpriteManager.Instance.GS("Plant_2");
            gm.finalStage = SpriteManager.Instance.GS("plant2");
            gm.Go(10f,"plant");

        }
        SelectionManager.Instance.DestroyHighlight();

    }

            public void DisplayPlantFlowerTT()
    {
        ToolTipManager.Instance.MouseOver("Plant a flower to be used for food.","Can only be planted on tilled land!");
    }


    public void BuildFloor()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;
            if (t.type != "grass")
                continue;


            // t.type = "floor";

            //WorldManager.Instance.floor
            JobManager.Instance.CreateJob(t,SpriteManager.Instance.GS("floor") ,"floor", 1f, false, .5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

            public void DisplayBuildFloorTT()
                {
                    ToolTipManager.Instance.MouseOver("Buid a floor that walls and furniture can be placed on.", "Can only be built on Dirt!");
                }

    public void BuildDoor()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;

            if (!CheckNeighborsBuildDoor(t))
                continue;

            if (t.type == "door")
                continue;


            //  t.type = "door";
           // WorldManager.Instance.door
           JobManager.Instance.CreateJob(t,SpriteManager.Instance.GS("door") ,"door", 1f, false, .5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

            public void DisplayBuildDoorTT()
            {
                ToolTipManager.Instance.MouseOver("Build a door to protect your homes!", "Must have 2 walls to connect to!");
            }


    public void DestoryTile()
    {
      //  Debug.Log("Destroy tile");

        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr.sprite == SpriteManager.Instance.GS("grass") )//WorldManager.Instance.grass)
                continue;
          

            // WorldManager.Instance.grass
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"), "grass", 1f, true, .5f);
            if(t.type == "door")
            {
                GameObject go2 = WorldManager.Instance.DoorTileDict[t];
                Debug.Log("door gameobject" + go2);
                WorldManager.Instance.DoorTileDict.Remove(t);
                Destroy(go2);
            }
            t.MovementSpeedAdjustment = 1f;
            t.type = "NewGrass";
        }
        SelectionManager.Instance.DestroyHighlight();

    }


            public void DisplayDestroyTileTT()
            {
                ToolTipManager.Instance.MouseOver("Destroy anything you have built!", "Will return that area to grass!");
            }


    public void BuildWall ()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;

            if (t.type != "floor")
                continue;

            Sprite wall = walls[15];
          //  t.type = "wall";
            JobManager.Instance.CreateJob(t, wall,"wall", 0f,true, .5f);
          //  Debug.Log("created job for wall");

        }
        SelectionManager.Instance.DestroyHighlight();
    }

            public void DisplayBuildWallTT()
            {
                ToolTipManager.Instance.MouseOver("Build walls and create a home!", "Walls can only be placed on floors!");
            }



    //.................................................................................


    public void CheckNeighborsDestory(Tile t)
    {
        List<Tile> tiles = new List<Tile>();

        tiles.Add( WorldManager.Instance.GetTileAT(t.x, t.y+1)); //top
        tiles.Add(WorldManager.Instance.GetTileAT(t.x + 1, t.y));//right
        tiles.Add(WorldManager.Instance.GetTileAT(t.x-1, t.y));//left
        tiles.Add(WorldManager.Instance.GetTileAT(t.x, t.y - 1));//bottom
      
        foreach(Tile tt in tiles)
        {
            GameObject go = WorldManager.Instance.TileToGameObjectMap[tt];
            if(tt.type == "wall")
            {
                 CheckNeighbors(tt, false);
            }

        }
        
    }



    public void CheckNeighbors(Tile t, bool fc)
    {
        if (t.type == "NewGrass")
        {
            CheckNeighborsDestory(t);
            t.type = "grass";
        }
        
        if (t.type != "wall" )
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
    
        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        foreach(Sprite s in walls)
        {
            if (t.type == "grass")
            {
                t.type = "grass";
                continue;
            }
                

            if (s.name == wall)
                sr.sprite = s;
         
        }
    }


    bool CheckNeighborsBuildDoor(Tile t)
    {

        Tile R = WorldManager.Instance.GetTileAT(t.x + 1, t.y);//right
        Tile L = WorldManager.Instance.GetTileAT(t.x - 1, t.y);//left

        Tile T = WorldManager.Instance.GetTileAT(t.x, t.y + 1); //top
        Tile B = WorldManager.Instance.GetTileAT(t.x, t.y - 1);//bottom
        if (R.type == "wall" && L.type == "wall")
        {
            t.type = "door";
            //WorldManager.Instance.door
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("door"), "door", 1f, false, .5f);
            Vector3 place = new Vector3(t.x, t.y, 0);
            GameObject go = Instantiate(Hdoor, place, Quaternion.identity);
             WorldManager.Instance.DoorTileDict.Add(t, go);
            return true;
        }
         

        if(T.type == "wall" && B.type == "wall")
        {
           
            t.type = "door";
          
            JobManager.Instance.CreateJob(t,SpriteManager.Instance.GS("door1") ,"door", 1f, false, .5f);
            Vector3 place = new Vector3(t.x, t.y, 0);
            GameObject go = Instantiate(Vdoor, place, Quaternion.identity);
            WorldManager.Instance.DoorTileDict.Add(t, go);
            return true;
        }

        else
          return false;
    }
}
