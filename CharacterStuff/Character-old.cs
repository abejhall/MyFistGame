﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Character : MonoBehaviour
{/*
    #region vars.....................................
    public float MoveSpeed = 3f;
    public Job MyJob = null;
    public Vector3 Dest;
    public Tile DestTile = null;
    public Tile NextTile;

    AudioSource aud;

    //door Stuff will be moved eventually
    public float DoorWait = 3f;
    public float jobwait = 70f;


    LooseMaterial MaterialsIAmHolding =null;


    private List<Job> _jobQueList;
    private List<GameObject> _greenHighlightsList;
    private List<GameObject> _redHighlightsList;
   
    public Path_AStar MyPathAStar;
    private Vector3 _tempCurTile;
    private Tile _currentTile; 
    public Vector3 NextTileV3; 
    public Vector3 CurrTileV3;
  

   private float _step;
   private float _startTime;


    Tile MatT;


   public  bool Islerping = false;

    //for debugging only 
    public bool MyJobIsNull;

    [Header("only public for debugging")]
    public bool gettingMaterial = false;
    public string Matt;
#endregion



   
    void Start()
    {
        aud = GetComponent<AudioSource>();

        NextTile = new Tile(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
        _jobQueList = JobManager.Instance.JobQueList;
        _greenHighlightsList = JobManager.Instance.GreenHighlightsList;
        _redHighlightsList = JobManager.Instance.RedHighlightsList;
        _startTime = Time.time;
    }
   

    // Update is called once per frame
    void Update()
    {
     
        //for debugging
        if (MyJob == null)
                MyJobIsNull = true;
        //for debugging
            if (MyJob != null)
                MyJobIsNull = false;

        if(MatT != null)
        {
            Matt = "TileAt"+MatT.x + "_" + MatT.y;
        }


        //get a job from job Que.
        GetAJobHippy();

        // If i am not already at my work site... get a path and goto my work site.
        GotoWork();

        // If job is all done,,  Clean up any extra highlights from being displayed
        CleanUpAfterMyself();


        _step = (Time.time - _startTime) * MoveSpeed;
        if (Islerping)
        {
            //switched the rotation of character sprite to face the way he is walking
            ChangeDirectionOfSpirte();

            transform.position = Vector3.Lerp(_tempCurTile,NextTileV3,_step);

           
        }
            
    }


    
    //switched the rotation of character sprite to face the way he is walking
    void ChangeDirectionOfSpirte()
    {
        if (_tempCurTile.x > NextTileV3.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }



    void GetAJobHippy()
    {
       

        if (JobManager.Instance.JobQueList.Count != 0 && MyJob == null)
        {
            WorldManager.Instance.world.tileGraph = null;
            Job j = JobManager.Instance.GetJob();
            MyJob = j;
            Tile t = j.jobTile;
            DestTile = j.jobTile;
            Dest = new Vector3(t.x, t.y, 0);

          

        }
        return;
    }

    void getPathFinding()
    {
        Debug.Log("Called GetPathFinding With these 2 tiles" + "_currentTile" 
            + _currentTile.y + "_" + _currentTile.y + "   " + DestTile.x+"_"+DestTile.y);

        MyPathAStar = new Path_AStar(WorldManager.Instance.world, //Copy of World
                                           _currentTile,  //Start tile
                                           DestTile); //Destination tile
    }

    void GotoWork()
    {
        if (MyJob == null)
            return;

       _currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);
       NextTileV3 = new Vector3(NextTile.x, NextTile.y, 0);
       CurrTileV3 = new Vector3(_currentTile.x, _currentTile.y, 0);
       _step = Time.time - Time.deltaTime * MoveSpeed;

       
     
        if (MyJob != null || _currentTile != DestTile )
        {
            //I do have a job and i am not at my destination and i a not at 0,0,0
           //Check if i have an A* Navigation already if not create it
            if(MyPathAStar == null)
            {
                getPathFinding();
                Debug.Log("got new path");
            }


            //Check if there is no path to my destination if not then cancle my job and clean up
            if( !CheckMyDestIsReachable() )
                {
                Debug.Log("dest not reachable");
                return;
                }

           // Debug.Log("number of mats" + MyJob.numberOfMats);
            

            //I now know i can reach my final destination
            if(MyJob.numberOfMats != 0 && gettingMaterial == false)
             {
                    gettingMaterial = true;

                    //find material for the job
                    if(MaterialsIAmHolding == null)
                {
                    MatT = StockPileManager.Instance.FindNeededMaterial(MyJob.jobMaterial);
                }
                   

                    Debug.Log("i should be pathfinding to this loc: " + MatT.x + "_" + MatT.y);

                //get pathfinding for new material

                if(MaterialsIAmHolding == null)
                {
                    DestTile = MatT;
                    MyPathAStar = null;


                    getPathFinding();
                }
                
                 




                    //make sure i can reach that material
                    if (!CheckMyDestIsReachable())
                    {
                        Debug.LogError("Cant Reach Material for Job Canceling Job");
                        JobManager.Instance.CancelJob(MyJob);
                        MyJob = null;

                                     
                    return;
                    }

                              
             }

           
                //set my nextTile if i dont have one.
                if (NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() != 0)
                {
                Debug.Log("i am in the Set my next tile section");


                NextTile = MyPathAStar.Dequeue();

               Debug.Log("I dequed this tile loc"+NextTile.x +"_"+NextTile.y);
               Debug.Log("My Path shows this many steps left: " + MyPathAStar.Length());

                    _tempCurTile = this.transform.position;
                    _startTime = Time.time;

                //I dont need any materials so just complete job;
                    if (NextTile == DestTile && !gettingMaterial)
                        StartCoroutine(MyDelayMethod(MyJob.timeToWait));

                    //I need matterails and i walked to where they were pick them up and set new destination
                    if(NextTile == DestTile && gettingMaterial)
                     {
                   
                        if (WorldManager.Instance.LooseMaterialsMap.ContainsKey(NextTile)  && gettingMaterial)
                        {
                            gettingMaterial = false;
                             LooseMaterial tmpLooseMats = WorldManager.Instance.LooseMaterialsMap[NextTile].GetComponent<LooseMaterial>();
                            Debug.Log("called SetLooseMatStats ");
                            DestTile = MyJob.jobTile;
                        _currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);
                           // MyPathAStar = null;
                            getPathFinding();
                            SetLooseMatStats(tmpLooseMats);

                            
                            
                            return;/////////////////////////////////////////////////////////////////////////////////////////
                        }
                    
                        
                   

                        Debug.Log("check to see what tile NExtTIle is "+NextTile.x + "_" + NextTile.y);
                  
                        if(NextTile == DestTile && MaterialsIAmHolding != null)
                    {
                        CompleteJob();

                    }
                        //MyPathAStar = null;
                       
                        
                    }

#region DoorStuff

                if (NextTile.type != "door" )
                            Islerping = true;

                        else if (NextTile.type == "door")
                    {

                        Islerping = false;


                        Invoke("WillPauseForDoor", DoorWait);
                        GameObject go = WorldManager.Instance.DoorTileDict[NextTile];
                        go.GetComponent<AnimationScriptDoor>().Open();


                    }

#endregion




            }
            


            


        
        }
        return;
    }

    void SetLooseMatStats(LooseMaterial tmpLooseMats)
    {
        Debug.Log("assigned stats to materialIAmHolding");
        //clone loose mats
        MaterialsIAmHolding = this.gameObject.AddComponent<LooseMaterial>();
        MaterialsIAmHolding.mySprite = tmpLooseMats.mySprite;
        MaterialsIAmHolding.myType = tmpLooseMats.myType;
        //MaterialsIAmHolding.MyCounterTotal = tmpLooseMats.MyCounterTotal;
        MaterialsIAmHolding.MaxStackSize = tmpLooseMats.MaxStackSize;
        MaterialsIAmHolding.CounterText = tmpLooseMats.CounterText;
        MaterialsIAmHolding.baseType = tmpLooseMats.baseType;

        if (MyJob.numberOfMats <= tmpLooseMats.MyCounterTotal)
        {
            tmpLooseMats.MyCounterTotal -= MyJob.numberOfMats;
            MaterialsIAmHolding.MyCounterTotal = MyJob.numberOfMats;
        }
    }



    bool CheckMyDestIsReachable()
    {
        if (NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() == 0)
        {
            // Debug.LogError("no path to destination");
            Vector3 _JobTileV = new Vector3(MyJob.jobTile.x, MyJob.jobTile.y, 0);
            JobManager.Instance.CancelJob(MyJob);

            MyPathAStar = null;
            Islerping = false;
            MyJob = null;


            foreach (GameObject g in _greenHighlightsList)
            {
                if (g.transform.position == _JobTileV)
                    SimplePool.Despawn(g);
            }


            return false;
        }
        return true;
    }





    void WillPauseForDoor()
    {
        Islerping = true;
    }

 

    void CleanUpAfterMyself()
    {
        if (_jobQueList != null && _jobQueList.Count == 0)
        {
            _greenHighlightsList.Clear();

            foreach (GameObject r in _redHighlightsList)
            {

                SimplePool.Despawn(r);

            }
        }
    }



    void CompleteJob()
    {

        Debug.Log("called Complete Job");
        if (MyJob == null)
           return;

        
        
        Tile t = MyJob.jobTile;
     
        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
        t.MovementSpeedAdjustment = MyJob.movementSpeedAdjustment;
        go.GetComponent<SpriteRenderer>().sprite = MyJob.jobSprite;
        t.type = MyJob.Type;
        
        BuildManager.Instance.CheckNeighbors(t,true);


        WorldManager.Instance.world.tileGraph = null;

        //look up and remove Green Highlight
        for (int i = 0; i < JobManager.Instance.GreenHighlightsList.Count; i++)
        {
            if(JobManager.Instance.GreenHighlightsList[i].transform.position == go.transform.position)
            {
                SimplePool.Despawn(JobManager.Instance.GreenHighlightsList[i]);
                JobManager.Instance.GreenHighlightsList.Remove(JobManager.Instance.GreenHighlightsList[i]);
            }
          
        }

       
        SoundManager.Instance.PlaySound("pop",aud);
        RoomTypeSetter(t);
        MyJob = null;
        Dest = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        LooseMaterial lm = GetComponent<LooseMaterial>();
        if(lm != null)
        Destroy(lm);

        gettingMaterial = false;
        MyPathAStar = null;
        

        //FIX ME: Not sure why I had to make this work around.... Cant find the logic error but i know i must of made one here.
        //This works for now but would rather do it right.
        if (JobManager.Instance.GreenHighlightsList.Count == 0)
        {
            GameObject[] goLeftOvers = GameObject.FindGameObjectsWithTag("GreenHL");
            foreach(GameObject gl in goLeftOvers)
            {
                SimplePool.Despawn(gl);
            }
        }
    }

   
    //For testing will come up with a better way later

    void RoomTypeSetter(Tile t)
    {
        if(t.type == "wall" || t.type == "door")
        {
            t.RoomFloor = false;
            t.RoomDevider = true; }

        if(t.type == "floor")
        {
            t.RoomFloor = true;
        }
    }

    IEnumerator MyDelayMethod(float delay)
    {
        SoundManager.Instance.PlaySound(MyJob.WorkSound, aud);

        yield return new WaitForSeconds(delay);
        
        CompleteJob();
    }

    */

}
