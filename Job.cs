using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job {

    public Tile jobTile;
    public Sprite jobSprite;
    public float timeToWait;
    public float movementSpeedAdjustment;
    public bool attachToOthers;
    public string Type;
    public string WorkSound;
    public string jobMaterial ="";
    public int numberOfMats =0;
    public bool IsHaulingJob = false;

    public Job(Tile t, Sprite s,string type, float time, string sound)
    {
        this.Type = type;
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
        WorkSound = sound;
       // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
    }


    //only for hauling jobs
    public Job(Tile t, Sprite s, string type, float time, string sound, string matType, int numberOfMats)
    {
        this.Type = type;
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
        this.WorkSound = sound;
        this.jobMaterial = matType;
        this.numberOfMats = numberOfMats;
        this.IsHaulingJob = true;

    }

    public Job(Tile t, Sprite s, float movement, bool _attachToOthers, float time, string sound)
    {
        this.movementSpeedAdjustment = movement;
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
        this.WorkSound = sound;
        // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
    }




}
