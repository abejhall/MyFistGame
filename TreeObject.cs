﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeObject : MonoBehaviour {



    public bool CheckingIfJobIsComplete = false;
    public bool ChoppedTree = true;

    public Sprite stage1;

    public int QuanityOfWood = 5;
    public Tile t;
    public SpriteRenderer sr;
    public GameObject Treetop;
    public string Type;

    float StartTime;
    public float timer;

    public GameObject go;

    Text CounterText = null;

    void Start()
    {


        t = WorldManager.Instance.GetTileAT(go.transform.position.x, go.transform.position.y);
        go = this.gameObject;
        sr = go.AddComponent<SpriteRenderer>();

      

        sr.sortingLayerName = "Struture";
        go = WorldManager.Instance.TreeTileDict[t];
        t.type = "tree";
        t.MovementSpeedAdjustment = 0;


    }

    public void Update()
    {
        if (sr != null && !CheckingIfJobIsComplete)
        {



            sr.sprite = SpriteManager.Instance.GS("STree");
            t.MovementSpeedAdjustment = 1;
            CheckingIfJobIsComplete = true;





        }

        if (t.type == "logs" && ChoppedTree == true)
        {
            sr.sprite = SpriteManager.Instance.GS("logs");
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();
            
            CounterText.text = QuanityOfWood.ToString();
            Destroy(Treetop);
            ChoppedTree = false;
        }


        if(CounterText != null)
        {
           CounterText.text = QuanityOfWood.ToString();

            if(CounterText.text == "0")
            {
                Destroy(gameObject);
            }
        }
        
    }

}