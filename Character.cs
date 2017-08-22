using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public Job MyJob = null;
    public Vector3 Dest;
    public Tile DestTile = null;
    public Tile NextTile;

    


    List<Job> jobQueList;
    List<GameObject> GreenHighlightsList;
    List<GameObject> RedHighlightsList;
   
    Path_AStar pathAStar;

    Tile currentTile; 
   public Vector3 NextTileV3; 
   public Vector3 CurrTileV3;
   public Vector3 TempCurTile;
          float step;
    float startTime;

     public  bool islerping = false;

    //for debugging only 
    public bool MyJobIsNull;

    public AudioClip pop;

    
    // Use this for initialization
    void Start()
    {
        NextTile = new Tile(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
        jobQueList = JobManager.Instance.JobQueList;
        GreenHighlightsList = JobManager.Instance.GreenHighlightsList;
        RedHighlightsList = JobManager.Instance.RedHighlightsList;
        startTime = Time.time;
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


        step = (Time.time - startTime) * MoveSpeed;
        if (islerping)
        {

            transform.position = Vector3.Lerp(TempCurTile,NextTileV3,step);

            Debug.Log("tc:" + TempCurTile + "NT:" + NextTileV3 + "step:" + step);
            Debug.Log("lerping between:" + CurrTileV3 + "and:" + NextTileV3);
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

    

    void GotoWork()
    {
        if (MyJob == null)
            return;

       currentTile = WorldManager.Instance.GetTileAT(this.transform.position.x, this.transform.position.y);
       NextTileV3 = new Vector3(NextTile.x, NextTile.y, 0);
       CurrTileV3 = new Vector3(currentTile.x, currentTile.y, 0);
       step = Time.time - Time.deltaTime * MoveSpeed;


        // I am at my destination and can complete job
        if (currentTile == DestTile)
        {
            islerping = false;
            CompleteJob();
            return;
        }

        if (MyJob != null || currentTile != DestTile )
        {
            //I do have a job and i am not at my destination and i a not at 0,0,0
           //Check if i have an A* Navigation already if not create it
            if(pathAStar == null)
            {
              
                pathAStar = new Path_AStar(WorldManager.Instance.world, //Copy of World
                                            currentTile,  //Start tile
                                            DestTile); //Destination tile

                Debug.Log("Called pathAStart with this showing as my dest tile:"+ DestTile.x +DestTile.y);
            }

            

            Debug.Log("This is where i see myself at"+ currentTile.x + currentTile.y);
           // Debug.Log("length of path in PathAStar; "+pathAStar.Length());

           
           
            
            //set my nextTile if i dont have one.
            if(NextTileV3 == CurrTileV3 && pathAStar != null && CurrTileV3 != Dest)
            {
               
                    NextTile = pathAStar.Dequeue();
                    TempCurTile = this.transform.position;
                startTime = Time.time;

              

                    islerping = true;
                   


            }





          
               


            //just for Debugging
            if (NextTile != currentTile && NextTile != null) 
            Debug.Log("Next tile in Q coords are"+NextTile.x +" "+ NextTile.y+ "and i am at:"+currentTile.x +" "+currentTile.y );








        
        }
        return;
    }


    void CleanUpAfterMyself()
    {
        if (jobQueList != null && jobQueList.Count == 0)
        {
            GreenHighlightsList.Clear();

            foreach (GameObject r in RedHighlightsList)
            {

                SimplePool.Despawn(r);

            }
        }
    }



    void CompleteJob()
    {

      
        if (MyJob == null)
           return;

        
        //FIXME: this is for testing only... place a blimish tile where once was grass.
        Tile t = MyJob.jobTile;
        GameObject go = WorldManager.Instance.TileToGameObjectMap[t]; 
        go.GetComponent<SpriteRenderer>().sprite = WorldManager.Instance.blimish;


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
        AudioSource.PlayClipAtPoint(pop, this.transform.position);
        MyJob = null;
        Dest = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        pathAStar = null;

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
