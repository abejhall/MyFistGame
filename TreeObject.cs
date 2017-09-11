using System.Collections;
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
            //we know the type is not logs so we change the spirte to logs
            sr.sprite = SpriteManager.Instance.GS("logs");

            //now add a counter to visualy show how many logs are stacked up here
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();
            
            //adjust the counter to show the appropriate amount of logs
            CounterText.text = QuanityOfWood.ToString();

            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, "logs");

            //clean up old tree stuff
            Destroy(Treetop);
            ChoppedTree = false;
        }

        //keep counter up to date.
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
