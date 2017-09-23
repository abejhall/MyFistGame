using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobManager : MonoBehaviour {

    //set up a singleton for this Manager.
    public static JobManager Instance;

    //these GameObjects are the prefabs of the 2 selection colors
    public GameObject NoMaterialMarker;
    public GameObject GreenSelectHighlight;
    public GameObject RedSelectHighLight;
    public GameObject RedTileMarker;
    public GameObject DisplayPanel;
    


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

    public void CreateJob(Tile t, Sprite s, string type, float movement, bool attachToOthers, float time, string sound, string jobMat, int numberOfMats, bool disableMovementblock = false )
    {
        if (disableMovementblock)
            t.MovementSpeedAdjustment = 1;
        Job j = new Job(t, s, movement, attachToOthers, time, sound);


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



            //assign type and movement speed and the add job to jobQuelist
            j.Type = type;
            j.movementSpeedAdjustment = movement;
            j.jobMaterial = jobMat;
            j.numberOfMats = numberOfMats;
            JobQueList.Add(j);


            //remove the left over yellow highlight
            DespawnYellowHighlight(t);

        }
    }
 
    public void CreateStockPileJob(Tile t, Sprite s, string type, float movement, bool attachToOthers, float time, string sound, string jobMat, int numberOfMats, bool disableMovementblock = false)
    {
      

        if (disableMovementblock)
            t.MovementSpeedAdjustment = 1;
        Job j = new Job(t, s,type,time, sound,jobMat,numberOfMats);
        j.IsHaulingJob = true;

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



            //assign type and movement speed and the add job to jobQuelist
            j.Type = type;
            j.movementSpeedAdjustment = movement;
            j.jobMaterial = jobMat;
            j.numberOfMats = numberOfMats;
            JobQueList.Add(j);


            //remove the left over yellow highlight
            DespawnYellowHighlight(t);

        }




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


    public bool DoesJobExistOnTile(Tile t)
    {
        foreach(Job j in JobQueList)
        {
            if (j.jobTile == t)
                return true;
        }


        return false;
    }

    public void RemoveAGreenHighLight(Tile t)
    {
        for (int i = 0; i < GreenHighlightsList.Count; i++)
        {
            Vector3 vec = new Vector3(t.x, t.y, 0);
            if (GreenHighlightsList[i].transform.position == vec)
                SimplePool.Despawn(GreenHighlightsList[i]);
        }
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

    public void CancelSelectedJobs(Tile t)
    {
        

        for (int i = 0; i < JobQueList.Count; i++)
        {
            if(JobQueList[i].jobTile == t)
            {
                for (int g = 0; g < GreenHighlightsList.Count; g++)
                {
                    if(GreenHighlightsList[g].transform.position == WorldManager.Instance.TileToGameObjectMap[t].transform.position)
                    {
                        SimplePool.Despawn(GreenHighlightsList[g]);
                    }
                }
                
                JobQueList.Remove(JobQueList[i]);
            }
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
    

    public Job GetJob()//Vector3 vec
    {

        Job j = JobQueList[0];
        JobQueList.Remove(JobQueList[0]);

        return j;

      


    }


    public void JobToHard(Tile t)
    {
        Vector3 tmpV = new Vector3(t.x, t.y, 0);
        SimplePool.Spawn(RedTileMarker, tmpV, Quaternion.identity);
        SoundManager.Instance.PlaySound("uhoh");
        WorldManager.Instance.world.tileGraph = null;
    }




}
