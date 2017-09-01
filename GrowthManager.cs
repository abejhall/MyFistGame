﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthManager :MonoBehaviour
{

   public bool timing = false;
   public bool CheckingIfJobIsComplete = false;

    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;
    public Sprite finalStage;

    public Tile t;
    public SpriteRenderer sr;

    public string Type;

    float StartTime;
    public float timer;

    GameObject go;
    

    public void Update()
    {
        if(sr != null && !CheckingIfJobIsComplete)
        {
            Debug.Log("sr sprite is:" + sr.sprite);
            
            if(sr.sprite == stage1)
            {
                startTimers();
                CheckingIfJobIsComplete = true;

            }
        }


        if(timing)
        {
            if (Time.time > (timer + StartTime) && Time.time < ((timer * 2) + StartTime))
            {
                sr.sprite = stage2;
                t.type = "sprout";
                
            }

            if (Time.time > ((timer * 3) + StartTime) && Time.time < ((timer * 4) + StartTime))
            {
                sr.sprite = stage3;
                t.type = "sprout";
            }


            if (Time.time > ((timer * 4) + StartTime))
            {
                t.type = Type;
                sr.sprite = finalStage;
                timing = false;
                Destroy(this);
            }
        }

    }

    public void Go( float timer, string type)
    {
      
       

            go = WorldManager.Instance.TileToGameObjectMap[t];
            sr = go.GetComponent<SpriteRenderer>();
            
            // t.type = type;

           // sr.sprite = stage1;
        
    }
     public void startTimers()
    {
        StartTime = Time.time;
        timing = true;

    }

}
