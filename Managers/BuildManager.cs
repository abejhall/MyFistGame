using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

   
    // empty GameObjects just to keep hiarcy clean
    public GameObject RockParent;
    public GameObject PlantParent;
    public GameObject TreeParent;

    public GameObject LooseMaterialPrefab;

    //prefab of counter that is spawned on loose items on the ground
    public GameObject ItemCounter;

    //prefabs for vert or horizontal doors
    public GameObject Vdoor;
    public GameObject Hdoor;

   // public Sprite[] walls;

    public Sprite[] doors;

    GrowthManager gm;


#region Singleton

    public static BuildManager Instance;
    // Use this for initialization
    void Awake () {
        Instance = this;

	}
    #endregion

    public void CancelJobsAtPoint()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            JobManager.Instance.CancelSelectedJobs(t);
        }
            
    }


    #region BuildButtons


    public void BuildTilledLand()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.type != "grass")
                continue;
            if (t.MovementSpeedAdjustment == 0f)
                continue;
           
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("Dirt"), "dirt", 1f, false, 7f,"dig","",0, false);

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
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("Plant_0"), "plant", 1f, false, 4f,"click","",0, false);

            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            gm = go.AddComponent<GrowthManager>();
            gm.t = t;
            gm.timer = 3f;
            gm.Type = "plant";
            gm.stage1 = SpriteManager.Instance.GS("Plant_0");
            gm.stage2 = SpriteManager.Instance.GS("Plant_1");
            gm.stage3 = SpriteManager.Instance.GS("Plant_2");
            gm.finalStage = SpriteManager.Instance.GS("plant2");
            gm.Go(10f, "plant");

        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void MineRocks()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 1f)
                continue;
            if (t.type != "rock")
                continue;



            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"), "rocks", 1f, false, 7f,"mining","",0, true);

        }
        SelectionManager.Instance.DestroyHighlight();
    }

    public void ChopTrees()
    {

        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
           if (t.type != "tree")
                continue;



            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"), "logs", 1f, false, 7f,"chopping","",0, true);

        }
        SelectionManager.Instance.DestroyHighlight();
    }

    public void PickFlowers()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.type != "plant")
                continue;



            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"), "plants", 1f, false, 5f,"sheers","",0, true);

        }
        SelectionManager.Instance.DestroyHighlight();
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
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("floor"), "floor", 1f, false, 5f,"click","rocks",5, false);

        }
        SelectionManager.Instance.DestroyHighlight();

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
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("door"), "door", 1f, false, 5f,"click","logs",5, false);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void DestoryTile()
    {
        //  Debug.Log("Destroy tile");

        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr.sprite == SpriteManager.Instance.GS("grass"))//WorldManager.Instance.grass)
                continue;


            // WorldManager.Instance.grass
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"), "grass", 1f, true, 5f,"pop", "", 0, true);
            if (t.type == "door")
            {
                GameObject go2 = WorldManager.Instance.DoorTileDict[t];
                Debug.Log("door gameobject" + go2);
                WorldManager.Instance.DoorTileDict.Remove(t);
                Destroy(go2);
            }
            // t.MovementSpeedAdjustment = 1f;
            t.type = "NewGrass";
        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void BuildWall()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;

            if (t.type != "floor")
                continue;

            Sprite wall = SpriteManager.Instance.GS("wall_");//walls[15];
                                                             //  t.type = "wall";
            JobManager.Instance.CreateJob(t, wall, "wall", 0f,  true, 5f, "pop","logs",5);
            //  Debug.Log("created job for wall");

        }
        SelectionManager.Instance.DestroyHighlight();
    }


    #endregion


#region Initialize Functions


    public void InitializeRocks(Tile t) 
    {

        GameObject go = new GameObject();
  
        go.transform.position = new Vector3(t.x, t.y, 0);
        go.name = "rock_" + t.x + "_" + t.y;
        go.transform.SetParent(RockParent.transform);
 
        WorldManager.Instance.RockTileDict.Add(t, go);
      
        RockObject ro =  go.AddComponent<RockObject>();
        go.GetComponent<RockObject>().go = go;
    
         
        
        int ran = Random.Range(0, 3);
        if (ran == 0)
           ro.stage1 = SpriteManager.Instance.GS("Rock");
        else if (ran == 1)
            ro.stage1 = SpriteManager.Instance.GS("rock2");
        else
            ro.stage1 = SpriteManager.Instance.GS("rock3");

        
        
        
        go.tag = "rock";
        
     
       
        t.MovementSpeedAdjustment = 0;
        t.type = "rock";
    }


    public void InitializePlants(Tile t)
    {
        GameObject go = new GameObject();
        go.transform.position = new Vector3(t.x, t.y, 0);
        go.name = "plant_" + t.x + "_" + t.y;
        go.transform.SetParent(PlantParent.transform);

        WorldManager.Instance.PlantTileDict.Add(t, go);
       
        PlantObject po = go.AddComponent<PlantObject>();
       
        go.GetComponent<PlantObject>().go = go;
    }


    public void InitializeTrees(Tile t)
    {

      
        GameObject go = new GameObject();
        go.transform.position = new Vector3(t.x, t.y, 0);
        go.name = "tree_" + t.x + "_" + t.y;
        go.transform.SetParent(TreeParent.transform);

        WorldManager.Instance.TreeTileDict.Add(t, go);

        TreeObject treeObject = go.AddComponent<TreeObject>();


        //add another sprite to the tile above the tree for the tree top
        GameObject goTreeTop = new GameObject();
        goTreeTop.name = "TreeTop" + t.x + "_" + t.y;
        
        treeObject.Treetop = goTreeTop;
        SpriteRenderer srTreeTop = goTreeTop.AddComponent<SpriteRenderer>();
        srTreeTop.sprite = SpriteManager.Instance.GS("treeTop");
        goTreeTop.transform.position = new Vector3(t.x, t.y+1, 0);
        goTreeTop.transform.SetParent(TreeParent.transform);
        srTreeTop.sortingLayerName = "trees";
        
        go.GetComponent<TreeObject>().go = go;
       
    }


    public void InitializeGrass(Tile t)
    {
        Sprite tmp;
        tmp = SpriteManager.Instance.GS("grass");
        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];

        go.GetComponent<SpriteRenderer>().sprite = tmp;
        t.type = "grass";

      
        
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
       
        sr.sprite = tmp;

        t.MovementSpeedAdjustment = 1;

    }


#endregion

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
        foreach(Sprite s in SpriteManager.Instance.SPRITES)//walls)
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
            JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("door"), "door", 1f, false, 5f,"pop","logs",5);
            Vector3 place = new Vector3(t.x, t.y, 0);
            GameObject go = Instantiate(Hdoor, place, Quaternion.identity);
             WorldManager.Instance.DoorTileDict.Add(t, go);
            return true;
        }
         

        if(T.type == "wall" && B.type == "wall")
        {
           
            t.type = "door";
          
            JobManager.Instance.CreateJob(t,SpriteManager.Instance.GS("door1") ,"door", 1f, false, 5f, "pop","logs",5);
            Vector3 place = new Vector3(t.x, t.y, 0);
            GameObject go = Instantiate(Vdoor, place, Quaternion.identity);
            WorldManager.Instance.DoorTileDict.Add(t, go);
            return true;
        }

        else
          return false;
    }
}
