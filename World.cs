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
           
            Sprite tmpSprite;
          
                tmpSprite = WorldManager.Instance.grass;
           

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
                    
                    if (t.y > 45 && t.y < 52 && t.x == 44)
                    {
                        tmpSprite = WorldManager.Instance.blimish;
                        t.MovementSpeedAdjustment = 0;
                        tileGraph = null;
                    }


            if (t.y > 45 && t.y < 52 && t.x == 47)
            {
                tmpSprite = WorldManager.Instance.blimish;
                t.MovementSpeedAdjustment = 0;
                tileGraph = null;
            }


            //FIXME: removed cause not used yet.

            GameObject go = WorldManager.Instance.TileToGameObjectMap[t];
            go.GetComponent<SpriteRenderer>().sprite = tmpSprite;
           // WorldManager.Instance.TileToSpriteMap.Add(t, tmpSprite);
        }

    }

}
