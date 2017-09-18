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

  //  Text CounterText = null; //warning says never used
    GameObject loosemat;
    void Start()
    {

        //cache a reference of the tile we are on
        t = WorldManager.Instance.GetTileAT(go.transform.position.x, go.transform.position.y);
        //cache a reference of the gameobject we are on
        go = this.gameObject;

        //add a sprite renderer and cache a reference 
        sr = go.AddComponent<SpriteRenderer>();

      
        //assign the sorting layer
        sr.sortingLayerName = "Struture";
        
        // add this tree to a dictionary
        go = WorldManager.Instance.TreeTileDict[t];
        //assign the type
        t.type = "tree";
        //make the movement speed 0 so you cant walk through it
        t.MovementSpeedAdjustment = 0;


    }

    public void Update()
    {
        //assign the sprite only once.  and stop trying after it is assigned
        if (sr != null && !CheckingIfJobIsComplete)
        {
            sr.sprite = SpriteManager.Instance.GS("STree");
            CheckingIfJobIsComplete = true;
        }

        //watch the type and if it changes to logs then remove the tree sprite and create a loose material of type logs and place it at this location
        //assign all the correct vars to the loose material and  then destroy ourself
        if (t.type == "logs" && ChoppedTree == true)
        {
            ChoppedTree = false;
           
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
       // string quanity = QuanityOfWood.ToString();
        //assign all the other properties of the loose material;
        loosemat.GetComponent<LooseMaterial>().CounterText = loosemat.GetComponentInChildren<Text>();
        loosemat.GetComponent<LooseMaterial>().MyCounterTotal = QuanityOfWood;
        loosemat.GetComponent<LooseMaterial>().mySprite = SpriteManager.Instance.GS("logs");
        loosemat.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GS("logs");
        loosemat.GetComponent<LooseMaterial>().myType = "logs";
        loosemat.GetComponent<LooseMaterial>().baseType = "grass";
        loosemat.GetComponent<LooseMaterial>().MaxStackSize = 50;
        loosemat.GetComponent<LooseMaterial>().MyTile =t;

        //we now have a seperate loose material we can destoy ourself
        Destroy(this.gameObject);
    }

}
