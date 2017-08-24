using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{

    public int x { get; protected set;}

    public int y { get; protected set; }

    public Sprite sprite = null;

    public bool IsWalkable = true;

    public float MovementSpeedAdjustment;

    public string type;

    public World world;

    public Tile(int x, int y,  bool iswalkable = true )
    {
       // GameObject go = new GameObject();
        this.x = x;
        this.y = y;
       
     
        this.IsWalkable = iswalkable;
       
        
      //  this.transform.position = new Vector3(x, y, 0);

        if (!IsWalkable)
        {
            MovementSpeedAdjustment = 0f;
        }
        else if(IsWalkable)
            MovementSpeedAdjustment = 1f;

        world = WorldManager.Instance.world;
     
    }




    public Tile[] GetNeighbours(bool diagOkay =false)
    {
        Tile[] ns;

        if (diagOkay == false)
        {
            ns = new Tile[4]; //Tile order N E S W
        }
        else
        {
            ns = new Tile[8]; // Tile Order N E S W NE SE SW NW
        }


        Tile n; // potential neighbour

        n = WorldManager.Instance.GetTileAT(x, y + 1);
        ns[0] = n;  //could be null but that's okay.
        n = WorldManager.Instance.GetTileAT(x+1, y);
        ns[1] = n;  //could be null but that's okay.
        n = WorldManager.Instance.GetTileAT(x, y-1);
        ns[2] = n;  //could be null but that's okay.
        n = WorldManager.Instance.GetTileAT(x-1, y);
        ns[3] = n;  //could be null but that's okay.


        if(diagOkay == true)
        {
            n = WorldManager.Instance.GetTileAT(x+1, y+1);
            ns[4] = n;  //could be null but that's okay.
            n = WorldManager.Instance.GetTileAT(x+1, y-1);
            ns[5] = n;  //could be null but that's okay.
            n = WorldManager.Instance.GetTileAT(x-1, y-1);
            ns[6] = n;  //could be null but that's okay.
            n = WorldManager.Instance.GetTileAT(x-1, y+1);
            ns[7] = n;  //could be null but that's okay.
        }
        return ns;
    }


}
