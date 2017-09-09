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
            sr.sprite = SpriteManager.Instance.GS("Rocks");
            GameObject Counter = GameObject.Instantiate(BuildManager.Instance.ItemCounter, transform.position, Quaternion.identity);
            Counter.transform.parent = this.transform;
            CounterText = Counter.GetComponentInChildren<Text>();
            CounterText.text = QuanityOfRocks.ToString();


            MinedToRocks = false;
        }

     
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
