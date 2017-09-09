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
        
           // go = WorldManager.Instance.PlantTileDict[t];
       
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
            sr.sprite = SpriteManager.Instance.GS("plants");
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();
            CounterText.text = QuanityOfPlants.ToString();

            PickedPlants = false;
        }

        
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