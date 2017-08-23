﻿using System.Collections;
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

    public AudioClip Pop;

    
    // Use this for initialization
    void Start()
    {
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

            transform.position = Vector3.Lerp(_tempCurTile,NextTileV3,_step);

            //Debug.Log("tc:" + _tempCurTile + "NT:" + NextTileV3 + "step:" + _step);
           // Debug.Log("lerping between:" + CurrTileV3 + "and:" + NextTileV3);
        }
            
    }


    void GetAJobHippy()
    {
       

        if (JobManager.Instance.JobQueList.Count != 0 && MyJob == null)
        {
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

       

        // I am at my destination and can complete job
        if (_currentTile == DestTile)
        {
            Islerping = false;
            CompleteJob();
            return;
        }

        if (MyJob != null || _currentTile != DestTile )
        {
            //I do have a job and i am not at my destination and i a not at 0,0,0
           //Check if i have an A* Navigation already if not create it
            if(MyPathAStar == null)
            {  getPathFinding();

               
                    
            }

           
           
            if (NextTileV3 == CurrTileV3 && MyPathAStar != null && CurrTileV3 != Dest && MyPathAStar.Length() == 0)
            {
                Debug.LogError("no path to destination");
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
                    _tempCurTile = this.transform.position;
                _startTime = Time.time;

              

                    Islerping = true;
                   


            }


        
        }
        return;
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

      
        if (MyJob == null)
           return;

        
        
        Tile t = MyJob.jobTile;
      //  Debug.Log("myJob's tile info is MovementSpeed:" + t.MovementSpeedAdjustment+", sprite:"+t.sprite + "X Y:" + t.x+t.y);
        GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
        t.MovementSpeedAdjustment = MyJob.movementSpeedAdjustment;
        go.GetComponent<SpriteRenderer>().sprite = MyJob.jobSprite;
        Debug.Log(MyJob.jobSprite);
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

        //Play sound effect when done
        AudioSource.PlayClipAtPoint(Pop, this.transform.position);
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

   

}
