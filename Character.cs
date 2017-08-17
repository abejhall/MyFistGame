using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public Job MyJob;
    public Vector3 Dest;

    List<Job> jobQueList;
    List<GameObject> GreenHighlightsList;
    List<GameObject> RedHighlightsList;
    


    public AudioClip pop;

    
    // Use this for initialization
    void Start()
    {
        jobQueList = JobManager.Instance.JobQueList;
        GreenHighlightsList = JobManager.Instance.GreenHighlightsList;
        RedHighlightsList = JobManager.Instance.RedHighlightsList;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (JobManager.Instance.JobQueList.Count != 0 && MyJob == null)
        {
            Job j = JobManager.Instance.GetJob();
            MyJob = j;
           

            Tile t = j.jobTile;   
            Dest = new Vector3(t.transform.position.x, t.transform.position.y, 0);
           
            

        }


        if (MyJob != null)
        {
            transform.position = Vector3.Lerp(transform.position, Dest, MoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, Dest) < .3f)
            {
                CompleteJob();
            }
        }

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
        Tile t = MyJob.jobTile;
        t.GetComponent<SpriteRenderer>().sprite = WorldManager.Instance.blimish;

        for (int i = 0; i < JobManager.Instance.GreenHighlightsList.Count; i++)
        {
            if(JobManager.Instance.GreenHighlightsList[i].transform.position == t.transform.position)
            {
                SimplePool.Despawn(JobManager.Instance.GreenHighlightsList[i]);
                JobManager.Instance.GreenHighlightsList.Remove(JobManager.Instance.GreenHighlightsList[i]);
            }
          
        }

        AudioSource.PlayClipAtPoint(pop, this.transform.position);
        MyJob = null;
        Dest = new Vector3(0,0,0);
       

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
