using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {


    List<Tile> FirstPotNeighborList;
    List<Tile> WallsOfPotentialRoom;
    List<Tile> SecondPotNeighborList;

	// Use this for initialization
	void Start () {
        FirstPotNeighborList = new List<Tile>();
        SecondPotNeighborList = new List<Tile>();
        WallsOfPotentialRoom = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckForRoom(Tile t)
    {
        Tile[] N = t.GetNeighbours();
        //check neighbors for type, see if it is a floortype
        for (int i = 0; i < N.Length; i++)
        {
            if(N[i].RoomFloor)
            {
                FirstPotNeighborList.Add(N[i]);
            }
        }
        // use this list of floors to check for a complete room; 
        //add all tiles to second list 
        //if neighbor is a wall door or floor keep checking
        for (int i = 0; i < FirstPotNeighborList.Count; i++)
        {
            if(!FirstPotNeighborList[i].RoomFloor || !FirstPotNeighborList[i].RoomDevider)
            {
                break;
            }
            else if (FirstPotNeighborList[i].RoomDevider)
            {
                WallsOfPotentialRoom.Add(SecondPotNeighborList[i]);
            }
            else if (FirstPotNeighborList[i].RoomFloor)
            {
                SecondPotNeighborList.Add(FirstPotNeighborList[i]);
            }
        }
        //dont check neighbors of walls or doors
        // add each tile we check in a list
        //if we never run into something that is not a floor 
    }

   
}
