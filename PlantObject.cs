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
            //we know the type is now plants so we change the spirte to plants
            sr.sprite = SpriteManager.Instance.GS("plants");

            //now add a counter to visualy show how many plants are stacked up here
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();

            //adjust the counter to show the appropriate amount of plants
            CounterText.text = QuanityOfPlants.ToString();

            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, "plants");

            PickedPlants = false;
        }

        //keep counter up to date.
        if (CounterText != null)
        {
            CounterText.text = QuanityOfPlants.ToString();

            if (CounterText.text == "0")
            {
                Destroy(gameObject);
            }
        }
    }

}