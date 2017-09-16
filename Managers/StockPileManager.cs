using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockPileManager : MonoBehaviour {



    public Toggle stone;
    public Toggle wood;
    public Toggle food;
    public Toggle weapons;
    public Toggle medicine;




    public GameObject BlueHighlight;

    public Dictionary<Tile, GameObject> StockPileMap;

    public bool StockPileMenueActive;

    //used to know when the blue highlighs have finished respawning to stop infinate loop
    bool BlueHighlightsRespawned = false;

    public TogglePannel tp;

    List<GameObject> ActiveBlueHighlights;

    //counter so that we can limit the number of times we itterate over all the stockpiles
    public float counter = 0f;
    public float counterWaitTime = 20f;
    bool PauseCreatingJobs = false;

    [Header("For DeBugging so I can keep track of number of loose materials")]
    public int NumberOfLoosItems =0;


    //will be used as bools for stockpiles
    public GameObject[] toggles;


#region Singleton
    public static StockPileManager Instance;
    private void Awake()
    {
        Instance = this;
    }
#endregion

    void Start () {
        StockPileMenueActive = tp.toggleStockPileButton;
        StockPileMap = new Dictionary<Tile, GameObject>();
        ActiveBlueHighlights = new List<GameObject>();
	}
	
	
	void Update () {

        NumberOfLoosItems = WorldManager.Instance.LooseMaterialsMap.Count;


        //this counter sets a delay for when it creates hauling jobs
        //this way it doesnt use cpu up each frame
#region DelayCounter
        counter += Time.deltaTime;

        if (counter >= counterWaitTime && !PauseCreatingJobs)
        {
            PauseCreatingJobs = true;
            CreateStockPileJobs();
        }
#endregion


        //change the bool when player has opened the stockpile menue
        StockPileMenueActive = tp.toggleStockPileButton;

        if (StockPileMenueActive)
            ShowAllStockPiles();
        else
            StopShowingStockPiles();

        //StopShowingStockPiles();

    }


    public void CreateStockPile()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            
            if (t.MovementSpeedAdjustment == 0)
                continue;

            //if there is already a stockpie here remove it
            if (StockPileMap.ContainsKey(t))
            {
                StockPile OldStockpile = WorldManager.Instance.TileToGameObjectMap[t].GetComponent<StockPile>();
                if(OldStockpile != null)
                {
                    Destroy(OldStockpile);
                    StockPileMap.Remove(t);
                }
                
            }
            //Add tile to Stockpile map
            StockPileMap.Add(t, null);
            t.IsStockPile = true;
            Debug.Log(t.type);

            //create the blue highlight above tile
         GameObject go = SimplePool.Spawn(BlueHighlight, new Vector3(t.x, t.y, 0), Quaternion.identity);
            ActiveBlueHighlights.Add(go);

            //add stockpile component to tile's gameobject and assign values
          StockPile sp =  WorldManager.Instance.TileToGameObjectMap[t].AddComponent<StockPile>();
            sp.stone = stone.isOn;
            sp.wood = wood.isOn;
            sp.weapons = weapons.isOn;
            sp.medicine = medicine.isOn;
            sp.food = food.isOn;

        }

        SelectionManager.Instance.DestroyHighlight();

        //check to see if i have a loose material on me if so assign it to my stockpile map
        foreach (Tile stockPileTile in StockPileMap.Keys)
        {
            if (WorldManager.Instance.LooseMaterialsMap.ContainsKey(stockPileTile))
            {
                if(StockPileMap[stockPileTile] != null)
                StockPileMap.Add(stockPileTile, WorldManager.Instance.LooseMaterialsMap[stockPileTile]);
            }
        }


    }

    public void ShowAllStockPiles()
    {
        //when stockpileMenueActive is true highlight all stockpiles in blue
        if (StockPileMenueActive && !BlueHighlightsRespawned)
        {
            foreach(Tile t in StockPileMap.Keys)
            {
                GameObject go = SimplePool.Spawn(BlueHighlight, new Vector3(t.x, t.y, 0), Quaternion.identity);
                ActiveBlueHighlights.Add(go);

            }

           
           
        }
        BlueHighlightsRespawned = true;
    }

   public void StopShowingStockPiles()
    {
        StockPileMenueActive = false;
        BlueHighlightsRespawned = false;
        //despawn blue highlights from view when stockpile menue is closed
        for (int i = 0; i < ActiveBlueHighlights.Count; i++)
        {
            SimplePool.Despawn(ActiveBlueHighlights[i]);
            ActiveBlueHighlights.Remove(ActiveBlueHighlights[i]);
        }
            
        
    }

    public void RemoveStockPile()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (StockPileMap.ContainsKey(t))
                StockPileMap.Remove(t);
                 t.IsStockPile = false;

            for (int i = 0; i < ActiveBlueHighlights.Count; i++)
            {
                GameObject go = ActiveBlueHighlights[i];
                
                if(go.transform.position.x == t.x && go.transform.position.y == t.y)
                {
                    ActiveBlueHighlights.Remove(go);
                    SimplePool.Despawn(go);
                }
            }

        }
    }

    void CreateStockPileJobs()
    {
        

        foreach(Tile t in StockPileMap.Keys)
        {
            //if stockpile was changed since last we looked bail out.
            if (!t.IsStockPile)
            {
                Debug.LogWarning("Someone removed this stockpile in the middle of me checking!");
                continue;
            }


            // check to see if i have already assigned a job for this tile
            //if i have bail move to the next tile
            if (JobManager.Instance.DoesJobExistOnTile(t))
            {
                Debug.LogWarning("this stockpile job already exist");
                continue;
            }
            else //there is no job for this tile so assign one
            {
                AssignJob(t);
            }
           
            //TODO:::::::::::::::::::::::::::::::::::::::::::::::::::
            //loop through all the stockpile map and check to see if it has an item on it
            //check to see if that item can hold more on it

            //check to make sure there is NOT already a job for this tile!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            
            //if there is room left either empty or stack not full loop through the worldmanagers loose materials map
            //and see if there is a free item to add to stack or place down if the tile is empty
            //create job to pick up item. then create job to move item to stockpile
            //may need to change the way character looks at jobs for this
        }
        counter = 0f;
        PauseCreatingJobs = false;

    }

    void AssignJob(Tile t)
    {
        int _stackToFil;

        //is there a loose item on this tile?
        if(WorldManager.Instance.LooseMaterialsMap.ContainsKey(t))
        {
            //there is a loose item on the tile so let us see if there is any room 
            //left to add more of the same item
            LooseMaterial lm = WorldManager.Instance.LooseMaterialsMap[t].GetComponent<LooseMaterial>();
            if(lm == null)
            {
                Debug.LogError("Loose Material Map says there is a loose item here but not showing up.");
            }
            else
            {   //checking if the stack is full if so bail
                if (lm.MyCounterTotal >= lm.MaxStackSize)
                    return;
                else //save how much we can still use.
                    _stackToFil = lm.MaxStackSize - lm.MyCounterTotal;

                //loop through all the loose materials to see if there is one of that type available
                foreach(Tile LooseMaterialTile in WorldManager.Instance.LooseMaterialsMap.Keys)
                {
                    if (LooseMaterialTile == t)
                        continue;
                    if (LooseMaterialTile.IsStockPile)
                        continue;
                    LooseMaterial lm2 = WorldManager.Instance.LooseMaterialsMap[LooseMaterialTile].GetComponent<LooseMaterial>();
                    if (lm2.myType != lm.myType) //check to see if the mats we are looking at are the same as what we are looking for 
                        continue;// they are not so we bail
                    else//they are what we are looking for assign job
                    {
                        JobManager.Instance.CreateStockPileJob(t, SpriteManager.Instance.GS(lm.baseType), lm.myType, 1f, false, 1f, "click", lm.myType, 5);//_stackToFil);
                    }
                }

            }



        }
        else
        {
           // JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS(lm.baseType), "stockpile", 1f, false, 1f, "click", lm.myType, 5);
        }


      //  JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"),"stockpile",1f,false,0f,"pop");
    }

    public Tile FindNeededMaterial(Job currentJob)
    {   
        string mat = currentJob.jobMaterial;
        foreach(Tile t in WorldManager.Instance.LooseMaterialsMap.Keys)
        {  if (t.IsStockPile && currentJob.IsHaulingJob)
                continue;

            LooseMaterial lm = WorldManager.Instance.LooseMaterialsMap[t].GetComponent<LooseMaterial>();
           
            if (lm.myType == mat && !lm.SomeOneIsComingForMe)
            {
                    lm.SomeOneIsComingForMe = true;
                    t.LooseMat = lm.gameObject;
                   // WorldManager.Instance.LooseMaterialsMap.Remove(t);
                    return t;
                
            }
            
        }

        Debug.LogError("material asked for was not found in looseMaterialsMap:" + mat);
        return null;
    }

}
