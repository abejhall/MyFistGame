using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{


    Tile[,] tiles;

    // The pathfinding graph used to navigate our world map.
    public Path_TileGraph tileGraph;

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public World(int width = 200, int height = 200)
    {

        Width = width;
        Height = height;



    }


    public void CreateWorld(int WorldHeight = 200, int WorldWidth = 200)
    {

        for (int x = 0; x < WorldHeight; x++)
        {
            for (int y = 0; y < WorldWidth; y++)
            {
                Tile t = new Tile(x, y);


                var go = new GameObject();
                go.name = "tile_" + x + "_" + y;
                WorldManager.Instance.TileToNameMap.Add(go.name, t);
                go.gameObject.AddComponent<SpriteRenderer>();

                go.transform.position = new Vector3(x, y, 0);

                WorldManager.Instance.TileToGameObjectMap.Add(t, go);

                go.transform.SetParent(WorldManager.Instance.transform);


            }
        }

        AssignTileTypeFirstTime();
    }

    void AssignTileTypeFirstTime()
    {
        foreach (Tile t in WorldManager.Instance.TileToNameMap.Values)
        {
            int ran = Random.Range(1, 1000);
            Sprite tmpSprite;

            tmpSprite = SpriteManager.Instance.GS("grass");// WorldManager.Instance.grass;

            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = tmpSprite;
            t.type = "grass";


            if (t.x == 0 || t.x >= 198 || t.y == 0 || t.y >= 198)
                continue;



            if (ran > 985)
            {
                // Debug.Log("running initRocks");
                if (BuildManager.Instance != null)
                    BuildManager.Instance.InitializeRocks(t);
                continue;
            }


            if (ran < 10)
            {
                //   Debug.Log("running initPlants");
                if (BuildManager.Instance != null)
                    BuildManager.Instance.InitializePlants(t);
            }


            if (ran > 10 && ran < 60)
            {
                //  Debug.Log("running initPlants");
                if (BuildManager.Instance != null)
                    BuildManager.Instance.InitializeTrees(t);
            }


        }

        CreateWaterBorder();
    }

    void CreateWaterBorder()
    {
        foreach (Tile t in WorldManager.Instance.TileToNameMap.Values)
        {


            //if (t.x == 0 || t.x == 99 || t.y == 0 || t.y == 99)
            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            if (t.x == 0)
            {
                sr.sprite = SpriteManager.Instance.GS("grasswaterL");
                t.MovementSpeedAdjustment = 0;
            }


            if (t.y == 0)
            {
                sr.sprite = SpriteManager.Instance.GS("grasswaterS");
                t.MovementSpeedAdjustment = 0;
            }

            if (t.x == 0 && t.y == 0)
            {
                sr.sprite = SpriteManager.Instance.GS("waterBL");
                t.MovementSpeedAdjustment = 0;
                Debug.Log("i make it this far");
            }


            if (t.x == 199)
            {
                sr.sprite = SpriteManager.Instance.GS("grasswaterR");
                t.MovementSpeedAdjustment = 0;
            }
            if (t.y == 199)
            {
                sr.sprite = SpriteManager.Instance.GS("grasswaterN");
                t.MovementSpeedAdjustment = 0;
            }


            if (t.x == 199 && t.y == 199)
            {
                sr.sprite = SpriteManager.Instance.GS("waterTR");
                t.MovementSpeedAdjustment = 0;
                Debug.Log("changed sprite on :" + t.x + "-" + t.y);
            }
            if (t.x == 199 && t.y == 0)
            {
                sr.sprite = SpriteManager.Instance.GS("waterBR");
                t.MovementSpeedAdjustment = 0;
                Debug.Log("changed sprite on :" + t.x + "-" + t.y);
            }

            if (t.x == 0 && t.y == 199)
            {
                sr.sprite = SpriteManager.Instance.GS("waterTL");
                t.MovementSpeedAdjustment = 0;
                Debug.Log("changed sprite on :" + t.x + "-" + t.y);
            }


        }








    }
}



   




     
           
      


