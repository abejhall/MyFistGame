using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job {

    public Tile jobTile;
    public Sprite jobSprite;
    public float timeToWait;


    public Job(Tile t, Sprite s, float f = .5f)
    {
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = f;
       // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
    }




}
