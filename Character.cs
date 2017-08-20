using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public Job MyJob = null;
    public Vector3 Dest;
    public Tile NextTile;

    public float JobDistComplete = .2f;


    List<Job> jobQueList;
    List<GameObject> GreenHighlightsList;
    List<GameObject> RedHighlightsList;

    Path_AStar pathAStar;

    public bool MyJobIsNull;

    public AudioClip pop;

    
    // Use this for initialization
    void Start()
    {
        NextTile = new Tile(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
        jobQueList = JobManager.Instance.JobQueList;
        GreenHighlightsList = JobManager.Instance.GreenHighlightsList;
        RedHighlightsList = JobManager.Instance.RedHighlightsList;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (MyJob == null)
            MyJobIsNull = true;

        if (MyJob != null)
            MyJobIsNull = false;

        //get a job from job Que.
        GetAJobHippy();

        // If i am not already at my work site... get a path and goto my work site.
        GotoWork();

        // If job is all done,,  Clean up any extra highlights from being displayed
        CleanUpAfterMyself();
    }


    void GetAJobHippy()
    {
        if (JobManager.Instance.JobQueList.Count != 0 && MyJob == null)
        {
            Job j = JobManager.Instance.GetJob();
            MyJob = j;

            
            Tile t = j.jobTile;
            Dest = new Vector3(t.x, t.y, 0);

          

        }
        return;
    }


    void GotoWork()
    {
        if (MyJob == null)
            return;

        if (MyJob != null || this.transform.position != new Vector3(Dest.x,Dest.y,0) && Dest != new Vector3(0,0,0))
        {
            //I do have a job and i am not at my destination and i a not at 0,0,0
           //Check if i have an A* Navigation already if not create it
            if(pathAStar == null)
            {
              
                pathAStar = new Path_AStar(WorldManager.Instance.world, //Copy of World
                    WorldManager.Instance.GetTileAT(Mathf.CeilToInt(this.transform.position.x),Mathf.CeilToInt(this.transform.position.y)),  //Start tile
                    WorldManager.Instance.GetTileAT(Mathf.CeilToInt(Dest.x),Mathf.CeilToInt(Dest.y))); //Destination tile

                Debug.Log("Called pathAStart with this showing as my dest tile:"+ Dest.x +Dest.y);
            }

            Tile t = WorldManager.Instance.GetTileAT(Mathf.CeilToInt(this.transform.position.x),Mathf.CeilToInt(this.transform.position.y));

            Debug.Log("This is what i am commpairing to as t to NextTile"+ t.x + t.y);
            Debug.Log("length of path in PathAStar; "+pathAStar.Length());

            if (pathAStar.Length() == 0 && pathAStar != null)
            {
                CompleteJob();
                pathAStar = null;

            }

            Vector3 tempNextileVector = new Vector3(NextTile.x, NextTile.y, 0);

            if(tempNextileVector != null && pathAStar != null)
            {
                if (Vector3.Distance(tempNextileVector, this.transform.position) < .3f)
                    NextTile = pathAStar.Dequeue();

            }



            //just for Debugging
            if (NextTile != WorldManager.Instance.GetTileAT(Mathf.CeilToInt(this.transform.position.x), Mathf.CeilToInt(this.transform.position.y))) ;
            Debug.Log("Next tile in Q coords are"+NextTile.x + NextTile.y);


            Vector3 NextTileV3 = new Vector3(NextTile.x, NextTile.y, 0);



            transform.position = Vector3.Lerp(transform.position, NextTileV3, MoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, NextTileV3) < JobDistComplete)
            {
                Tile curTile = WorldManager.Instance.GetTileAT(Mathf.FloorToInt( transform.position.x), Mathf.FloorToInt(transform.position.y));
                

                if(MyJob != null && NextTile != null && curTile == MyJob.jobTile  )
                {
                   // if (MyJob != null)
                      CompleteJob();
                }
                return;
            }
        }
        return;
    }


    void CleanUpAfterMyself()
    {
        if (jobQueList.Count == 0)
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
