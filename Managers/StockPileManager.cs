using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPileManager : MonoBehaviour {

    public GameObject BlueHighlight;

    public Dictionary<Tile, string> StockPileMap;

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
        StockPileMap = new Dictionary<Tile, string>();
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
            StockPileMap.Add(t, "");

         GameObject go = SimplePool.Spawn(BlueHighlight, new Vector3(t.x, t.y, 0), Quaternion.identity);
            ActiveBlueHighlights.Add(go);

        }

        SelectionManager.Instance.DestroyHighlight();
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
            if (t.type == "stockpile")
                continue;
           // Debug.Log("created job for this tile: Tile" + t.x + "_" + t.y);
            if(!JobManager.Instance.DoesJobExistOnTile(t))
            AssignJob(t);
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
      //  JobManager.Instance.CreateJob(t, SpriteManager.Instance.GS("grass"),"stockpile",1f,false,0f,"pop");
    }

    public Tile FindNeededMaterial(string mat)
    {
        foreach(Tile t in WorldManager.Instance.LooseMaterialsMap.Keys)
        {
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
