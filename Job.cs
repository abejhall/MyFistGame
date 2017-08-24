using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job {

    public Tile jobTile;
    public Sprite jobSprite;
    public float timeToWait;
    public float movementSpeedAdjustment;
    public bool attachToOthers;

    public Job(Tile t, Sprite s, float time = .5f)
    {
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
       // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
    }


    public Job(Tile t, Sprite s, float movement, bool _attachToOthers, float time = .5f)
    {
        this.movementSpeedAdjustment = movement;
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
        // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
    }




}
