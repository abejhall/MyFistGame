using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WorldManager : MonoBehaviour {

    
    public static WorldManager Instance { get; protected set; }

    public int WorldHeight = 100;
    public int WorldWidth = 100;


  

    //FIXME:
    //This will not be a permanate solution
    //Just hardcoding for prototype
    public Sprite grass;
    public Sprite plant;
    public Sprite blimish;

    public World world;

    //Used to store a map for what sprite is displayed on each tile
    public Dictionary<Tile, Sprite> TileToSpriteMap;
    public Dictionary<string, Tile> TileToNameMap;

	// Use this for initialization
	void Start () {

        //singleton of WorldManager
        Instance = this;


        //Initalize our map of tiles to sprites
        TileToSpriteMap = new Dictionary<Tile, Sprite>();
        
        //Initalize our map of tiles to tile names
        TileToNameMap = new Dictionary<string, Tile>();

        //creates the tiles for the first time
        CreateWorld();
	}
	
	
    //creates the tiles for the first time 
    void CreateWorld()
    {
        world.CreateWorld(100, 100);
    }

    public Tile GetTileAT(int x, int y)
    {
        if( x > WorldWidth || x < 0 || y > WorldHeight || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        if (TileToNameMap.ContainsKey("tile_" + x + "_" + y))
        {
            Tile t = TileToNameMap["tile_" + x + "_" + y];
            return t;
        }
        return null;
    }

    // This assigns the type of tile when the world is created for the first time.
 

}
