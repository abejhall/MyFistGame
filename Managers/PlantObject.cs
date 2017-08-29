using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantObject : MonoBehaviour
{


   
    public bool CheckingIfJobIsComplete = false;
    public bool PickedPlants = true;

    public Sprite stage1;

    public int QuanityOfPlants = 5;
    public Tile t;
    public SpriteRenderer sr;

    public string Type;

    float StartTime;
    public float timer;

    public GameObject go;

    void Start()
    {
      

        t = WorldManager.Instance.GetTileAT(go.transform.position.x, go.transform.position.y);
        go = this.gameObject;
        sr = go.AddComponent<SpriteRenderer>();

        sr.sortingLayerName = "HighLights";
        go = WorldManager.Instance.PlantTileDict[t];
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
            sr.sprite = SpriteManager.Instance.GS("plant");
            PickedPlants = false;
        }

    }




}