using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Character : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public Job MyJob = null;
    public Vector3 Dest;
    public Tile DestTile = null;
    public Tile NextTile;

    AudioSource aud;

    //door Stuff will be moved eventually
    public float DoorWait = 3f;
    public float jobwait = 70f;
    
   



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

   public  bool Islerping = false;

    //for debugging only 
    public bool MyJobIsNull;

  

    
    // Use this for initialization
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
            Job j = JobManager.Instance.GetJob();//transform.position
            MyJob = j;

            
           
            
            Tile t = j.jobTile;
            DestTile = j.jobTile;
            Dest = new Vector3(t.x, t.y, 0);

          

        }
        return;
    }

    void getPathFinding()
    {
       // _currentTile = WorldManager.Instance.GetTileAT((int)this.transform.position.x, (int)this.transform.position.y);
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

            }

           
           
            if (NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() == 0)
            {
               // Debug.LogError("no path to destination");
                Vector3 _JobTileV = new Vector3(MyJob.jobTile.x, MyJob.jobTile.y,0);
                JobManager.Instance.CancelJob(MyJob);
               
                MyPathAStar = null;
                Islerping = false;
                MyJob = null;

                
                foreach(GameObject g in _greenHighlightsList)
                {
                    if (g.transform.position == _JobTileV)
                        SimplePool.Despawn(g);
                }


                return;
            }
             

          

           
           
            
            //set my nextTile if i dont have one.
            if(NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() != 0)
            {
               
                    NextTile = MyPathAStar.Dequeue();
               // Debug.Log(NextTile.type);
                    _tempCurTile = this.transform.position;
                _startTime = Time.time;


                if (NextTile == DestTile)
                    StartCoroutine(MyDelayMethod(MyJob.timeToWait));

                if (NextTile.type != "door" || NextTile.type != "rock")
                    Islerping = true;

                else if (NextTile.type == "door")
                {
                   
                    Islerping = false;


                    Invoke("WillPauseForDoor", DoorWait);
                        GameObject go = WorldManager.Instance.DoorTileDict[NextTile];
                        go.GetComponent<AnimationScriptDoor>().Open();
                    

                }
               
               
                 

                  

            }


        
        }
        return;
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

       
        SoundManager.Instance.PlayPopSound();
        RoomTypeSetter(t);
        MyJob = null;
        Dest = new Vector3(this.transform.position.x, this.transform.position.y, 0);
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



}
