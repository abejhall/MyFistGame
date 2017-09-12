﻿using System.Collections;
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

    public Job(Tile t, Sprite s,string type, float time, string sound)
    {
        this.Type = type;
        this.jobTile = t;
        this.jobSprite = s;
        this.timeToWait = time;
        WorkSound = sound;
       // Debug.Log("Job Created:" + t+"_" + s+"_" + f);
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
