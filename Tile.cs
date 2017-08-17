using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int x { get; protected set;}

    public int y { get; protected set; }

    public Sprite sprite = null;

    public bool IsWalkable = true;

    public float MovementSpeedAdjustment = 0f;


    public Tile(int x, int y,  bool iswalkable = true )
    {
       // GameObject go = new GameObject();
        this.x = x;
        this.y = y;
       
     
        this.IsWalkable = iswalkable;
       
        
        this.transform.position = new Vector3(this.x, this.y, 0);
    }

}
