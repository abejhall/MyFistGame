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
            //we know the type is not rocks so we change the spirte to rocks
            sr.sprite = SpriteManager.Instance.GS("Rocks");

            //now add a counter to visualy show how many rocks are stacked up here
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();

            //adjust the counter to show the appropriate amount of rocks
            CounterText.text = QuanityOfRocks.ToString();

            //Add the loose material to the woldmanager dictionary to keep track of it
            WorldManager.Instance.LooseMaterialsMap.Add(t, "rocks");


            MinedToRocks = false;
        }

        //keep counter up to date.
        if (CounterText != null)
        {
            CounterText.text = QuanityOfRocks.ToString();

            if (CounterText.text == "0")
            {
                Destroy(gameObject);
            }
        }









    }

  


}
