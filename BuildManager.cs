using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}





    public void BuildTilledLand()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f  )
                continue;

            JobManager.Instance.CreateJob(t, WorldManager.Instance.blimish, 0f, .5f);
            
        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void PlantFlower()
    {
        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 0f)
                continue;

            JobManager.Instance.CreateJob(t, WorldManager.Instance.plant, 0f,.5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

    public void DestoryTile()
    {
        Debug.Log("Destroy tile");

        foreach (Tile t in SelectionManager.Instance.SelectedTileList)
        {
            if (t.MovementSpeedAdjustment == 1f)
                continue;

            t.MovementSpeedAdjustment = 1f;
            JobManager.Instance.CreateJob(t, WorldManager.Instance.grass, 1f, .5f);

        }
        SelectionManager.Instance.DestroyHighlight();

    }

}
