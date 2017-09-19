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
    GameObject loosemat;
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
           // t.MovementSpeedAdjustment = 1;
            CheckingIfJobIsComplete = true;





        }

        if (t.type == "logs" && ChoppedTree == true)
        {
            ChoppedTree = false;
            //we know the type is not logs so we change the spirte to logs
            //sr.sprite = SpriteManager.Instance.GS("logs");
            loosemat = GameObject.Instantiate(BuildManager.Instance.LooseMaterialPrefab, transform.position, Quaternion.identity);
            loosemat.name = "LooseLogs";
            UpDateLooseMatSettings();
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = loosemat.transform;



            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, loosemat);

            //clean up old tree stuff
            Destroy(Treetop);
            
        }

       
        
    }

    void UpDateLooseMatSettings()
    {

        //adjust the counter to show the appropriate amount of plants
        string quanity = QuanityOfWood.ToString();
        loosemat.GetComponent<LooseMaterial>().CounterText = loosemat.GetComponentInChildren<Text>();
        loosemat.GetComponent<LooseMaterial>().MyCounterTotal = QuanityOfWood;
        loosemat.GetComponent<LooseMaterial>().mySprite = SpriteManager.Instance.GS("logs");
        loosemat.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GS("logs");
        loosemat.GetComponent<LooseMaterial>().myType = "logs";
        loosemat.GetComponent<LooseMaterial>().baseType = "grass";
        loosemat.GetComponent<LooseMaterial>().MaxStackSize = 50;
        loosemat.GetComponent<LooseMaterial>().MyTile =t;
        Destroy(this.gameObject);
    }

}
