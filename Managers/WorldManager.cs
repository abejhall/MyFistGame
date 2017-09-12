using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WorldManager : MonoBehaviour {

    
    public static WorldManager Instance { get; protected set; }

    public int WorldHeight = 100;
    public int WorldWidth = 100;



    public World world;

    //used to keep track of the each room;
    // public List<Room> Rooms;
    #region Dictionaries to keep track of the stuff in the world

    //this will keep track of all the lose materials in the world for construction
    public Dictionary<Tile, GameObject> LooseMaterialsMap;

    // used to store a dictionary from tiles to the tile name
    public Dictionary<string, Tile> TileToNameMap;

    //used to store a dictionary give a tile and get the gameobject at that point in the game
    public Dictionary<Tile, GameObject> TileToGameObjectMap;

    //this is to keep track of door objects currently placed in the world this may be
    // replaced with one that keeps track of all things above the main tile
    public Dictionary<Tile, GameObject> DoorTileDict;

    //this keeps track of rocks on top of tiles
    public Dictionary<Tile, GameObject> RockTileDict;

    //this keeps track of plants on top of tiles
    public Dictionary<Tile, GameObject> PlantTileDict;

    //this keeps track of plants on top of tiles
    public Dictionary<Tile, GameObject> TreeTileDict;

#endregion
    // Use this for initialization
    void Start () {

        

        //singleton of WorldManager
        Instance = this;

        world = new World();


        // Rooms = new List<Room>();

#region Initalize Dictionaries
        //Initalize our map of loose materials and the tiles they sit on
        LooseMaterialsMap = new Dictionary<Tile, GameObject>();

        //Initalize our map of tiles to tile names
        TileToNameMap = new Dictionary<string, Tile>();

        //initalize our tile to gameobject map
        TileToGameObjectMap = new Dictionary<Tile, GameObject>();

        //initalize our tile to gameobject map for the doors only
        DoorTileDict = new Dictionary<Tile, GameObject>();

        //initalize our tile to gameobject map for the Rocks only
        RockTileDict = new Dictionary<Tile, GameObject>();

        //initalize our tile to gameobject map for the Plants only
        PlantTileDict = new Dictionary<Tile, GameObject>();

        //initalize our tile to gameobject map for the Trees only
        TreeTileDict = new Dictionary<Tile, GameObject>();

#endregion


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
            //FIXME: 
           // Debug.Log("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        if (TileToNameMap.ContainsKey("tile_" + x + "_" + y))
        {
            Tile t = TileToNameMap["tile_" + x + "_" + y];
            return t;
        }
        return null;
    }

    public Tile GetTileAT(float fx, float fy)
    {
        int x = Mathf.RoundToInt(fx);
        int y = Mathf.RoundToInt(fy);

        if (x > WorldWidth || x < 0 || y > WorldHeight || y < 0)
        {
            //FIXME: 
            // Debug.Log("Tile (" + x + "," + y + ") is out of range.");
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
