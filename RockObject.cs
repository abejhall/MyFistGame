using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockObject : MonoBehaviour
{
    


    public bool timing = false;
    public bool CheckingIfJobIsComplete = false;
    public bool MinedToRocks = true;

    public Sprite stage1;

    public int QuanityOfRocks = 5;
    public Tile t;
    public SpriteRenderer sr;

    public string Type;

    float StartTime;
    public float timer;

    public GameObject go;

    Text CounterText = null;
    GameObject loosemat;


    // Use this for initialization
    void Start()
    {
        t = WorldManager.Instance.GetTileAT(go.transform.position.x, go.transform.position.y);
        go = this.gameObject;
        sr = go.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "rocks";
        go = WorldManager.Instance.RockTileDict[t];
        t.type = "rock";
        t.MovementSpeedAdjustment = 0;
        

    }

    public void Update()
    {
        if (sr != null && !CheckingIfJobIsComplete)
        {
            

            if (t.type == "rock")
            {
                sr.sprite = stage1;
                t.MovementSpeedAdjustment = 0;
                 CheckingIfJobIsComplete = true;

            }



        }

        if (t.type == "rocks" && MinedToRocks == true)
        {
            MinedToRocks = false;


            loosemat = GameObject.Instantiate(BuildManager.Instance.LooseMaterialPrefab, transform.position, Quaternion.identity);
            loosemat.name = "LooseRocks";
            UpDateLooseMatSettings();
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = loosemat.transform;

            

            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, loosemat);


            
        }









    }

    void UpDateLooseMatSettings()
    {
        Debug.Log("called UpdateSettings");
        //adjust the counter to show the appropriate amount of plants
        string quanity = QuanityOfRocks.ToString();
        loosemat.GetComponent<LooseMaterial>().CounterText = loosemat.GetComponentInChildren<Text>();
        loosemat.GetComponent<LooseMaterial>().MyCounterTotal = QuanityOfRocks;
        loosemat.GetComponent<LooseMaterial>().mySprite = SpriteManager.Instance.GS("rocks");
        loosemat.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GS("rocks");
        loosemat.GetComponent<LooseMaterial>().myType = "rocks";
        loosemat.GetComponent<LooseMaterial>().baseType = "grass";
        loosemat.GetComponent<LooseMaterial>().MaxStackSize = 25;
        loosemat.GetComponent<LooseMaterial>().MyTile = t;
        Destroy(this.gameObject);
    }


}
