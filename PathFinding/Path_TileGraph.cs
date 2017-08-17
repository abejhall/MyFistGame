using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_TileGraph {

    //this class constructs a simple path-finding compatilbe graph 
    // of our world.  Each tile is node.  Each Walkable neighbour
    // from a tile is linked via an edge connection.

    Dictionary<Tile, Path_Node<Tile>> nodes;


	public Path_TileGraph(World world)
    {
        // Loop through all tiles of the world
        // for each tile create a node
        // Do we create nodes for non-floor tiles? noo for now
        // do we create for tiles that are completly unwalkable? no for now.,,.


        nodes = new Dictionary<Tile, Path_Node<Tile>>();

        for (int x  = 0; x  < world.Width; x ++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Tile t = WorldManager.Instance.GetTileAT(x, y);

                if(t.IsWalkable)
                {
                    Path_Node<Tile> n = new Path_Node<Tile>();
                    n.data = t;
                    nodes.Add(t, n);

                }
            }
        }

        // now loop through all the nodes a second time
        //create edges for neighbours

        foreach (Tile t in nodes.Keys)
        {

            Path_Node<Tile> n = nodes[t];

            List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();

            //get a list of neighbors for the tile
            Tile[] neighbours = t.GetNeighbours(true); // some of the array spots could be null
                                                       // if neightbor is walkable, create an edge to the relevant node.

            for (int i = 0; i < neighbours.Length; i++)
            {
                if(neighbours[i] != null && neighbours[i].IsWalkable)
                {
                    //this neighbour exists and is walkable.  so create an edge
                    Path_Edge<Tile> e = new Path_Edge<Tile>();
                    e.Cost = neighbours[i].MovementSpeedAdjustment;
                    e.node = nodes[neighbours[i]];

                    // Add the edge to our temorary (and growable) list
                    edges.Add(e);

                }
            }

            n.edges = edges.ToArray();
        }

    }
}
