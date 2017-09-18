using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CharacterStateMachine : MonoBehaviour {


    public enum StateMachine { Idle, GetJob, CanIReachJobAndMaterial, MoveToMaterial,PickUpMaterial, MoveToJob, CompleteJob,Working, DefendMyself, FindFood, sleep }
#region Vars.................
    AudioSource aud;

    public float MoveSpeed = 3f;
   [Header("For Tracking Movement")]
    public Vector3 Dest;
    public Vector3 NextTileV3;
    public Vector3 CurrTileV3;
    
    //door Stuff will be moved eventually
    public float DoorWait = 3f;
    public float jobwait = 70f;


    [Header("For Debugging")]
    public bool MyJobIsNull;
    public bool IHaveAMatT;
   
    public string Matt;



    [Header("State Machine Current State!")]
    public StateMachine CurrentState;


    // public vars not shown in inspector
    public LooseMaterial MaterialsIAmHolding = null;
    public Path_AStar MyPathAStar;
    public Tile DestTile = null;
    public Job MyJob = null;
    public Tile NextTile;


    //Private Vars

        #region Private vars
        bool RunCompleteJob = false;
        private float _step;
        private float _startTime;
        private Vector3 _tempCurTile;
        private Tile _currentTile;
        private List<Job> _jobQueList;
        private List<GameObject> _greenHighlightsList;
        private List<GameObject> _redHighlightsList;
        Tile StockTile = null;
        Tile MatT;
        float lastStateChage = 0f;
     public   float jobTimer;
        bool jobstart = true;
    public float ranJobWait;
    #endregion

    #endregion


    Thread thread;



    void Start () {

        ranJobWait = Random.Range(1f, 25f);

        SetCurrentState(StateMachine.Idle);

        _currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);
         NextTile = _currentTile;

        aud = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

#region UpDate Outside of State Machine
        //place methods that need to run every frame no matter what the state of the state machine
        ChangeDirectionOfSpirte();

        DebuggingHelpers();

       

#endregion
        //Character's State Machine......
        switch (CurrentState)
        {
            #region Idle State
            case StateMachine.Idle:

                
                if (JobManager.Instance.JobQueList.Count != 0)
                    SetCurrentState(StateMachine.GetJob);

                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Get Job State
            case StateMachine.GetJob:

                if (MyJob == null && JobManager.Instance.JobQueList.Count != 0)
                {
                   
                    if(jobstart)
                    {
                        jobTimer = Time.time;
                        jobstart = false;
                       

                    }

                    if(Time.time - jobTimer  >= ranJobWait)
                    {
                        WorldManager.Instance.world.tileGraph = null;
                        Job j = JobManager.Instance.GetJob();
                        MyJob = j;
                        Tile t = j.jobTile;
                        DestTile = j.jobTile;
                        Dest = new Vector3(t.x, t.y, 0);
                        jobstart = true;
                    }
                   
                }
                 //   MyJob = JobManager.Instance.GetJob();

                if (MyJob == null && JobManager.Instance.JobQueList.Count == 0)
                    SetCurrentState(StateMachine.Idle);

                if (MyJob != null)
                {

                    if (MyJob.numberOfMats > 0)
                        SetCurrentState(StateMachine.CanIReachJobAndMaterial);

                    if (MyJob.numberOfMats == 0)
                    {
                       // Debug.Log("testing multithread");
                       // thread = new Thread(getPathFinding);
                      // thread.Start();

                      getPathFinding();
                       SetCurrentState(StateMachine.MoveToJob);
                    }
                        

                }

                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            # region Can I Reach Job And Material State
            case StateMachine.CanIReachJobAndMaterial:

                //check to make sure i can reach my final destination
                DestTile = MyJob.jobTile;
                _currentTile = WorldManager.Instance.GetTileAT(transform.position.x, transform.position.y);
                getPathFinding();

                if (!CheckMyDestIsReachable())
                {
                    ThisJobIsToHard();
                }

                //check to see what materials i need and see if they are available

                StockTile = StockPileManager.Instance.FindNeededMaterial(MyJob);
                if(StockTile == null)
                {
                   
                    JobManager.Instance.RemoveAGreenHighLight(MyJob.jobTile);
                    NoMaterialForJob();
                   // Debug.Log("No StockTile bailing out and back to idle");
                    SetCurrentState(StateMachine.Idle);
                    break;
                }

                //check to see if i can reach the materials tile and then set my destination to that tile
                DestTile = StockTile;
                _currentTile = WorldManager.Instance.GetTileAT(transform.position.x, transform.position.y);
               // Debug.Log("DestTile shows:" + DestTile.x + "_" + DestTile.y);
               // Debug.Log("_currentTile shows:" + _currentTile.x + "_" + _currentTile.y);
                getPathFinding();

                if (!CheckMyDestIsReachable())
                {
                    ThisJobIsToHard();
                }

                //I now know i can reach everything so change my State to move to material and get material for job.
                SetCurrentState(StateMachine.MoveToMaterial);


                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Move To Material State
            case StateMachine.MoveToMaterial:
                //lerp to material
                //move to 1 tile away from that tile.
                
                _step = (Time.time - _startTime) * MoveSpeed;

                //move to 1 tile away from that tile.
                if (NextTile == DestTile)
                {
                    RunCompleteJob = true;
                    SetCurrentState(StateMachine.PickUpMaterial);
                }


                if (_currentTile == NextTile && !thread.IsAlive|| transform.position == NextTileV3 && !thread.IsAlive)
                {
                    if (MyPathAStar.Length() != 0)
                        NextTile = MyPathAStar.Dequeue();
                    _startTime = Time.time;
                    GetTempCurVect3();
                    _currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);


                    NextTileV3 = new Vector3(NextTile.x, NextTile.y, 0);

                }
                transform.position = Vector3.Lerp(_tempCurTile, NextTileV3, _step);

                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Pick Up Material State

            case StateMachine.PickUpMaterial:

                //make a copy of those material on this character.
                //remove the amount i need to complete this job.
                //remove that material from the list 
               
                   if(StockTile != null)
                {
                    GameObject StockGo = WorldManager.Instance.LooseMaterialsMap[StockTile];
                    LooseMaterial tmpLooseMats = StockGo.GetComponent<LooseMaterial>();
                    tmpLooseMats.SomeOneIsComingForMe = false;
                    SetLooseMatStats(tmpLooseMats);

                }
                else
                {
                    Debug.LogError("When I looked where the Mat tile was i found a null");
                }
              

                

               
                if(MaterialsIAmHolding != null)
                {
                    //check to see if i can reach the materials tile and then set my destination to that tile
                    DestTile =MyJob.jobTile;
                    _currentTile = WorldManager.Instance.GetTileAT(transform.position.x, transform.position.y);
                    getPathFinding();

                    SetCurrentState(StateMachine.MoveToJob);
                }
                

                if (MaterialsIAmHolding == null)
                {
                    Debug.LogError("i got to my material tried to pick it up but my MaterialIAmHolding is showing null!!!");
                    SetCurrentState(StateMachine.Idle);
                }

                    break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Move To Job State
            case StateMachine.MoveToJob:
                //check to see if the job is reachable
                if (!CheckMyDestIsReachable())
                {
                    ThisJobIsToHard();
                }
                //lerp to Job Site
                _step = (Time.time - _startTime) * MoveSpeed;

                //move to 1 tile away from that tile.
                if (NextTile == DestTile)
                {
                    RunCompleteJob = true;
                    SetCurrentState(StateMachine.CompleteJob);
                }
                    

                if (_currentTile == NextTile && !thread.IsAlive || transform.position == NextTileV3 && !thread.IsAlive)
                {
                    if(MyPathAStar.Length() != 0)
                    NextTile = MyPathAStar.Dequeue();
                    _startTime = Time.time;
                    GetTempCurVect3();
                    _currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);
                  
                    
                    NextTileV3 = new Vector3(NextTile.x, NextTile.y, 0);
                   
                }


                transform.position = Vector3.Lerp(_tempCurTile, NextTileV3, _step);


                break;
                #endregion
                /////////////////////////////////////////////////////////////////////
                #region Complete Job State

               
            case StateMachine.CompleteJob:

                if (RunCompleteJob)
                {
                   // Debug.Log("I was call to run complete job coroutine");
                    RunCompleteJob = false;
                   StartCoroutine(MyDelayMethod(MyJob.timeToWait));

                    SetCurrentState(StateMachine.Working);
                }
                


                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Working State
            case StateMachine.Working:
                //Do Nothing till state is changed.
                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Defend Myself State
            case StateMachine.DefendMyself:
                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Find Food State
            case StateMachine.FindFood:
                break;
            #endregion
            /////////////////////////////////////////////////////////////////////
            #region Sleep State
            case StateMachine.sleep:
                break;
            #endregion



        }











    }// END UPDATE////////////////////////////////////////////////////////////////////////////////



    void DebuggingHelpers()
    {
        //for debugging
        if (MyJob == null)
            MyJobIsNull = true;
        //for debugging
        if (MyJob != null)
            MyJobIsNull = false;

        //for debugging
        if (MatT == null)
            IHaveAMatT = true;
        //for debugging
        if (MatT != null)
            IHaveAMatT = false;

        if (MatT != null)
        {
            Matt = "TileAt" + MatT.x + "_" + MatT.y;
        }

    }


    void CompleteJob()
    {

      //  Debug.Log("called Complete Job");
        if (MyJob == null)
            return;

        Tile t = MyJob.jobTile;

        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
        t.MovementSpeedAdjustment = MyJob.movementSpeedAdjustment;
        go.GetComponent<SpriteRenderer>().sprite = MyJob.jobSprite;
        t.type = MyJob.Type;
        if (t.type == "floor")
        {
            t.BaseType = "floor";
        }
        if(t.type == "grass")
        {
            t.BaseType = "grass";
        }
        LooseMaterial lm = GetComponent<LooseMaterial>();
       // Debug.Log("Myjob shows this for isstockpile:"+MyJob.jobTile.IsStockPile);
        if(MyJob.jobTile.IsStockPile)
        {
            DropStockPile(t);
        }


        BuildManager.Instance.CheckNeighbors(t, true);
        
        WorldManager.Instance.world.tileGraph = null;

        //look up and remove Green Highlight
        for (int i = 0; i < JobManager.Instance.GreenHighlightsList.Count; i++)
        {
            if (JobManager.Instance.GreenHighlightsList[i].transform.position == go.transform.position)
            {
                SimplePool.Despawn(JobManager.Instance.GreenHighlightsList[i]);
                JobManager.Instance.GreenHighlightsList.Remove(JobManager.Instance.GreenHighlightsList[i]);
            }

        }

        SoundManager.Instance.PlaySound("pop", aud);
       
        MyJob = null;
        Dest = new Vector3(this.transform.position.x, this.transform.position.y, 0);
       
        if (lm != null)
            Destroy(lm);

       
        MyPathAStar = null;


        //FIX ME: Not sure why I had to make this work around.... Cant find the logic error but i know i must of made one here.
        //This works for now but would rather do it right.
        if (JobManager.Instance.GreenHighlightsList.Count == 0)
        {
            GameObject[] goLeftOvers = GameObject.FindGameObjectsWithTag("GreenHL");
            foreach (GameObject gl in goLeftOvers)
            {
                SimplePool.Despawn(gl);
            }
        }

        SetCurrentState(StateMachine.Idle);
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


    public void DropStockPile(Tile t)
    {
        Debug.Log("I dropped my load ");
        if (!WorldManager.Instance.LooseMaterialsMap.ContainsKey(t))
        {
            Vector3 locV3 = new Vector3(t.x, t.y, 0);

            GameObject loosemat = GameObject.Instantiate(BuildManager.Instance.LooseMaterialPrefab, locV3, Quaternion.identity);
            loosemat.name = "LooseMat";
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, locV3, Quaternion.identity);
            Counter.transform.parent = loosemat.transform;
            SpriteRenderer sr = loosemat.GetComponent<SpriteRenderer>();
            LooseMaterial lm = loosemat.GetComponent<LooseMaterial>();
            sr.sprite = SpriteManager.Instance.GS(MaterialsIAmHolding.myType);


           
            

            lm.MyCounterTotal = MaterialsIAmHolding.MyCounterTotal;
                  lm.mySprite = MaterialsIAmHolding.mySprite;
                    lm.MyTile = t;
                    lm.myType = MaterialsIAmHolding.myType;
              lm.MaxStackSize = MaterialsIAmHolding.MaxStackSize;
                  lm.baseType = t.BaseType;

            MaterialsIAmHolding = null;


            WorldManager.Instance.LooseMaterialsMap.Add(t,loosemat);
        }
        else //this tile has a stockpile on it already
        {
          LooseMaterial  lm = WorldManager.Instance.LooseMaterialsMap[t].GetComponent<LooseMaterial>();
            lm.MyCounterTotal += MaterialsIAmHolding.MyCounterTotal;
            lm.SomeOneIsComingForMe = false;
            MaterialsIAmHolding = null;

            GameObject go = WorldManager.Instance.LooseMaterialsMap[t];

            WorldManager.Instance.LooseMaterialsMap.Remove(t);
            WorldManager.Instance.LooseMaterialsMap.Add(t, go);


        }

    }



    void SetLooseMatStats(LooseMaterial tmpLooseMats)
    {
        Debug.Log("assigned stats to materialIAmHolding");
        //clone loose mats
        MaterialsIAmHolding = this.gameObject.AddComponent<LooseMaterial>();
        MaterialsIAmHolding.mySprite = tmpLooseMats.mySprite;
        MaterialsIAmHolding.myType = tmpLooseMats.myType;
        MaterialsIAmHolding.MaxStackSize = tmpLooseMats.MaxStackSize;
        MaterialsIAmHolding.CounterText = tmpLooseMats.CounterText;
        MaterialsIAmHolding.baseType = tmpLooseMats.baseType;

        if (MyJob.numberOfMats <= tmpLooseMats.MyCounterTotal)
        {
            tmpLooseMats.MyCounterTotal -= MyJob.numberOfMats;
            MaterialsIAmHolding.MyCounterTotal = MyJob.numberOfMats;
        }
    }

    void SetCurrentState(StateMachine state)
    {
        CurrentState = state;
        lastStateChage = Time.time;
    }

    float GetStateElapsed()
    {
        return Time.time - lastStateChage;
    }

    void ChangeDirectionOfSpirte()
    {
        if (_tempCurTile.x > NextTileV3.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    bool CheckMyDestIsReachable()
    {
        if (NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() == 0)
        {
            // Debug.LogError("no path to destination");
            Vector3 _JobTileV = new Vector3(MyJob.jobTile.x, MyJob.jobTile.y, 0);
            JobManager.Instance.CancelJob(MyJob);

            MyPathAStar = null;
           
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

    void getPathFinding()
    {
        // Debug.Log("testing multithread");
         thread = new Thread(getPathFinding2);
         thread.Start();

        // Debug.Log("Called GetPathFinding With these 2 tiles" + "_currentTile"
        //     + _currentTile.y + "_" + _currentTile.y + "   " + DestTile.x + "_" + DestTile.y);

        // MyPathAStar = new Path_AStar(WorldManager.Instance.world, //Copy of World
        //                                    _currentTile,  //Start tile
        //                                    DestTile); //Destination tile
    }

    void getPathFinding2()
    {
        // Debug.Log("Called GetPathFinding With these 2 tiles" + "_currentTile"
        //     + _currentTile.y + "_" + _currentTile.y + "   " + DestTile.x + "_" + DestTile.y);

        MyPathAStar = new Path_AStar(WorldManager.Instance.world, //Copy of World
                                           _currentTile,  //Start tile
                                           DestTile); //Destination tile
    }

    void ThisJobIsToHard()
    {
        JobManager.Instance.JobToHard(MyJob.jobTile);
        MyJob = null;
        SetCurrentState(StateMachine.Idle);
    }

    void NoMaterialForJob()
    {
       
        GameObject NoMat = JobManager.Instance.NoMaterialMarker;

        SimplePool.Spawn(NoMat, new Vector3(MyJob.jobTile.x,MyJob.jobTile.y, 0),Quaternion.identity);
        MyJob = null;
        
    }

    void GetTempCurVect3()
    {
        _tempCurTile = this.transform.position;
        CurrTileV3 = this.transform.position;
    }


    IEnumerator MyDelayMethod(float delay)
    {
        SoundManager.Instance.PlaySound(MyJob.WorkSound, aud);

        yield return new WaitForSeconds(delay);

        CompleteJob();
    }




}
