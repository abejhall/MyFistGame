using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour {

    //set up a singleton for this Manager.
    public static JobManager Instance;

    //these GameObjects are the prefabs of the 2 selection colors
    public GameObject GreenSelectHighlight;
    public GameObject RedSelectHighLight;


    //these list will keep track of the red and green highlights to be removed later by a function
    public List<GameObject> GreenHighlightsList;
    public List<GameObject> RedHighlightsList;

    public List<Job> JobQueList;

    public int JobsInQue= 0;

	// Use this for initialization
	void Awake () {
        //set up singleton for JobManager
        Instance = this;

        // Initalize lists 
        JobQueList = new List<Job>();


        GreenHighlightsList = new List<GameObject>();
        RedHighlightsList = new List<GameObject>();
      
	}


    private void Update()
    {
        //Debug.Log("greenHighlights list count:" + GreenHighlightsList.Count );
        JobsInQue = JobQueList.Count;
    }



    //FIXME:  TODO:

    public void CreateJob(Tile t, Sprite s, float movment, float time)
    {
        Job j = new Job(t, s, movment,time);


        // if a job already exist at cords bail out
        if (!CheckIfJobExist(j))
        {

            return;
        }


        //create a temp var for the tile at specific job and spawn a green highlight over it
        GameObject Go1 = WorldManager.Instance.TileToGameObjectMap[j.jobTile];
        if (Go1.GetComponent<SpriteRenderer>().sprite != null)
        {
            GameObject go = (GameObject)SimplePool.Spawn(GreenSelectHighlight, new Vector3(t.x, t.y, 0), Quaternion.identity);
            go.transform.SetParent(SelectionManager.Instance.PoolManager.transform);
            GreenHighlightsList.Add(go);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "HighLights";

            JobQueList.Add(j);



            DespawnYellowHighlight(t);

        }
    }

    public void CreateJob(Tile t, Sprite s)
    {
     

        Job j = new Job(t, s);


        // if a job already exist at cords bail out
        if(!CheckIfJobExist(j))
        {
            
            return;
        }


        //create a temp var for the tile at specific job and spawn a green highlight over it
        GameObject Go1 = WorldManager.Instance.TileToGameObjectMap[j.jobTile];
        if(Go1.GetComponent<SpriteRenderer>().sprite != null)
        {
            GameObject go =(GameObject) SimplePool.Spawn(GreenSelectHighlight, new Vector3(t.x, t.y, 0),Quaternion.identity);
            go.transform.SetParent(SelectionManager.Instance.PoolManager.transform);
            GreenHighlightsList.Add(go);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "HighLights";

            JobQueList.Add(j);

            
            
            DespawnYellowHighlight(t);

        }
        /*
        else if (Go1.GetComponent<SpriteRenderer>().sprite == WorldManager.Instance.plant)
        {
            if (!CheckIfJobExist(j))
            {
                Debug.Log("I am returning");
                return;
            }
            GameObject go = (GameObject)SimplePool.Spawn(RedSelectHighLight, new Vector3(t.x, t.y, 0), Quaternion.identity);
            go.transform.SetParent(SelectionManager.Instance.PoolManager.transform);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "HighLights";
            RedHighlightsList.Add(go);
            string tmp = "H_" + t.x + "_" + t.y;
            foreach (GameObject Hgo in SelectionManager.Instance.YellowGOList)
                {
                    if (Hgo.name == tmp)
                        SimplePool.Despawn(Hgo);
                }

        }

        */

    }


    bool CheckIfJobExist(Job j)
    {
        foreach(Job jqj in JobQueList)
        {
            GameObject Go2 = WorldManager.Instance.TileToGameObjectMap[jqj.jobTile];
            GameObject Go3 = WorldManager.Instance.TileToGameObjectMap[j.jobTile];
            // Debug.Log(jqj.jobTile +"is the name of the tile ");
            if (Go3.transform.position == Go2.transform.position)
                return false;
        }
        


        return true;
    }







    void DespawnYellowHighlight(Tile t)
    {
        string tmp = "H_" + t.x + "_" + t.y;
        foreach (GameObject Hgo in SelectionManager.Instance.YellowGOList)
        {
            if (Hgo.name == tmp)
                SimplePool.Despawn(Hgo);
        }
    }

    public void CancelJobAtLocation(Vector3 loc)
    {
        foreach(Job j in JobQueList)
        {
            GameObject Go = WorldManager.Instance.TileToGameObjectMap[j.jobTile];
            if (loc == Go.transform.position)
                JobQueList.Remove(j);
        }
    }


    public void CancelJob(Job j)
    {
        DespawnYellowHighlight(j.jobTile);
        JobQueList.Remove(j);
    }


    public void ReturnJobToQue(Job j)
    {
        JobQueList.Add(j);
    }
    

    public Job GetJob()
    {
        Job j = JobQueList[0];
        JobQueList.Remove(JobQueList[0]);
       
        return j;
    }

}
