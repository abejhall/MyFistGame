using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
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


        tiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {for (int y = 0; y < Height; y++)
            {
                tiles[x, y] = new Tile(x, y);
            }
        }

        Debug.Log("World created with " + (Width * Height) + " tiles.");
              
    }


    public void CreateWorld(int WorldHeight = 100, int WorldWidth= 100)
    {

        for (int x = 0; x < WorldHeight; x++)
        {
            for (int y = 0; y < WorldWidth; y++)
            {

                var go = new GameObject().AddComponent<Tile>();
                go.name = "tile_" + x + "_" + y;
                WorldManager.Instance.TileToNameMap.Add(go.name, go);
                go.gameObject.AddComponent<SpriteRenderer>();

                go.transform.position = new Vector3(x, y, 0);

                go.transform.SetParent(WorldManager.Instance.transform);


            }
        }

        AssignTileTypeFirstTime();
    }

    void AssignTileTypeFirstTime()
    {
        foreach (Tile t in WorldManager.Instance.TileToNameMap.Values)
        {
            int rm = Random.Range(0, 100);
            Sprite tmpSprite;
            if (rm > 98)
                tmpSprite = WorldManager.Instance.blimish;
            else if (rm < 10)
                tmpSprite = WorldManager.Instance.plant;
            else
                tmpSprite = WorldManager.Instance.grass;

            t.GetComponent<SpriteRenderer>().sprite = tmpSprite;
            WorldManager.Instance.TileToSpriteMap.Add(t, tmpSprite);
        }

    }

}
