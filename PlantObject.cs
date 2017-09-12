using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantObject : MonoBehaviour
{

    
   
    public bool CheckingIfJobIsComplete = false;
    public bool PickedPlants = true;

    Text CounterText = null;

    public int QuanityOfPlants = 5;
    public Tile t;
    public SpriteRenderer sr;

    public string Type;

    float StartTime;
    public float timer;

    public GameObject go;
    GameObject loosemat;
    void Start()
    {

        go = this.gameObject;
        t = WorldManager.Instance.GetTileAT(go.transform.position.x, go.transform.position.y);
        
        sr = go.AddComponent<SpriteRenderer>();

        sr.sortingLayerName = "HighLights";
                         
        t.type = "plant";
        t.MovementSpeedAdjustment = 0;


    }

    public void Update()
    {
        if (sr != null && !CheckingIfJobIsComplete)
        {

            sr.sprite = SpriteManager.Instance.GS("plant2");
            sr.sortingLayerName = "plants";
            t.MovementSpeedAdjustment = 1;
            CheckingIfJobIsComplete = true;

        }

        if (t.type == "plants" && PickedPlants == true)
        {

            PickedPlants = false;
            //now add a counter to visualy show how many plants are stacked up here
            loosemat = GameObject.Instantiate(BuildManager.Instance.LooseMaterialPrefab, transform.position, Quaternion.identity);
            loosemat.name = "LoosePlants";
            UpDateLooseMatSettings();
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = loosemat.transform;
           

           
            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, loosemat);

            PickedPlants = false;
           
        }

    }

    void UpDateLooseMatSettings()
    {

        //adjust the counter to show the appropriate amount of plants
        string quanity = QuanityOfPlants.ToString();
        loosemat.GetComponent<LooseMaterial>().CounterText = loosemat.GetComponentInChildren<Text>();
        loosemat.GetComponent<LooseMaterial>().MyCounterTotal = QuanityOfPlants;
        loosemat.GetComponent<LooseMaterial>().mySprite = SpriteManager.Instance.GS("plants");
        loosemat.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GS("plants");
        loosemat.GetComponent<LooseMaterial>().myType = "plants";
        loosemat.GetComponent<LooseMaterial>().baseType = "grass";
        loosemat.GetComponent<LooseMaterial>().MaxStackSize = 50;
        Destroy(this.gameObject);
    }
}