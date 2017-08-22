using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{

 
        Tile[,] tiles;

        // The pathfinding graph used to navigate our world map.
        public Path_TileGraph tileGraph;

        public int Width{ get; protected set; }
            public int Height { get; protected set; }

        public World(int width = 100, int height = 100)
        {

            Width = width;
            Height = height;

        /*
            tiles = new Tile[Width, Height];

            for (int x = 0; x < Width; x++)
            {for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tile(x, y);
                }
            }
            */
          //  Debug.Log("World created with " + (Width * Height) + " tiles.");

        }

          
    public void CreateWorld(int WorldHeight = 100, int WorldWidth= 100)
    {

        for (int x = 0; x < WorldHeight; x++)
        {
            for (int y = 0; y < WorldWidth; y++)
            {
                Tile t = new Tile(x,y);
                

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
            //int rm = Random.Range(0, 100);
            Sprite tmpSprite;
           // if (rm > 98)
          //      tmpSprite = WorldManager.Instance.blimish;
          //  else if (rm < 10)
          //  {
          //      tmpSprite = WorldManager.Instance.plant;
          //      t.MovementSpeedAdjustment = 0f;
          //  }
            //  else
                tmpSprite = WorldManager.Instance.grass;
            //44_51 - 54_51 make plants

            if(t.x > 44 && t.x < 54 && t.y == 51)
            {
                tmpSprite = WorldManager.Instance.blimish;
                t.MovementSpeedAdjustment = 0;
                tileGraph = null;
            }

            if (t.x > 44 && t.x < 54 && t.y == 46)
            {
                tmpSprite = WorldManager.Instance.blimish;
                t.MovementSpeedAdjustment = 0;
                tileGraph = null;
            }
                    /*
                    if (t.y > 45 && t.y < 52 && t.x == 44)
                    {
                        tmpSprite = WorldManager.Instance.blimish;
                        t.MovementSpeedAdjustment = 0;
                        tileGraph = null;
                    }

                    */

            //FIXME: removed cause not used yet.

            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            go.GetComponent<SpriteRenderer>().sprite = tmpSprite;
           // WorldManager.Instance.TileToSpriteMap.Add(t, tmpSprite);
        }

    }

}
