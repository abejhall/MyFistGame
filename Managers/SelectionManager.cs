using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    //creating a singleton for the Selection Manager
    public static SelectionManager Instance { get; protected set; }

    //Empty Object for pooled items to parent to.  Only to keep inspector clean.
    public GameObject PoolManager;

    public bool mouseOverButton;

    //prefab for highlighted area
    public GameObject SelectedHighLight;
        
    // Var to store specific SelectedHighLight when looping
    GameObject currentHighlight;

    //Position vars
    Vector3 curFramePosition;
    Vector3 startDragPosition;

    //lists
    public List<GameObject> YellowGOList;
    public List<Tile> SelectedTileList;
    List<GameObject> PreviewHighLightTiles;

    // Use this for initialization
    void Start() {
        //making sure this is the only selection manager;
        Instance = this;

        mouseOverButton = false;


        YellowGOList = new List<GameObject>();
        SelectedTileList = new List<Tile>();
        PreviewHighLightTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {

        curFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curFramePosition.z = 0;

        //FIXME:
        //for testing only
        //ConvertTileToBlimish();


        if (!mouseOverButton)
        {
            DragStart();
            DragPreview();
            DragEnd();
        }
       

        //if you click the right mouse button clear highlighted objects
        if (Input.GetMouseButtonDown(1))
            DestroyHighlight();
    }


    public void MouseOverButton()
    {
        mouseOverButton = true;
    }

    public void MouseOFfButton()
    {
        mouseOverButton = false;
    }


    // if left mouse button is pushed down clear the list of highlighed objects. Get the selected tile under
    // mouse position. Then store that position to be used when the drag is released
    void DragStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DestroyHighlight();


            GameObject go = WorldManager.Instance.TileToGameObjectMap[SelectTile(curFramePosition)];
            Vector3 CurTilelocation = go.transform.position;
            startDragPosition = CurTilelocation;
        }

    }

    public Tile GetTileUnderMouse()
    {
        Tile t = SelectTile(curFramePosition);
        return t;
    }



    void DragPreview()
    {
        if (Input.GetMouseButton(0))
        {
            //while you are holding the left button down take the first object out of the pool of 
            //highlighted tiles and despawn it.  
            while (PreviewHighLightTiles.Count > 0)
            {
                GameObject go = PreviewHighLightTiles[0];
                SimplePool.Despawn(go);
                PreviewHighLightTiles.RemoveAt(0);

            }

            // convert float location to int location
            int start_x = Mathf.FloorToInt(startDragPosition.x);
            int end_x = Mathf.FloorToInt(curFramePosition.x);

           

            //invert if going right to left
            if (end_x < start_x)
            {
                int tmp = end_x;
                end_x = start_x;
                start_x = tmp;
            }
            
            // convert float location to int location
            int start_y = Mathf.FloorToInt(startDragPosition.y);
            int end_y = Mathf.FloorToInt(curFramePosition.y);

            //invert if going right to left
            if (end_y < start_y)
            {
                int tmp = end_y;
                end_y = start_y;
                start_y = tmp;
            }

           


            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldManager.Instance.GetTileAT(x, y);
                    if (t != null)
                    {
                        GameObject go = (GameObject)SimplePool.Spawn(SelectedHighLight, new Vector3(x, y, 0), Quaternion.identity);
                        PreviewHighLightTiles.Add(go);
                        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                        sr.sortingLayerName = "HighLights";
                        go.transform.SetParent(PoolManager.transform);
                       

                    }
                }
            }
        }




    }
    
    
    
    
    
    //loop through all the tiles under where we started the drag till we ended the drag
    // then spawn a highlight object above it
    // Then add the tiles underneath the highlights to a public list of tiles to be used for jobs.
    void DragEnd()
    {
        if (Input.GetMouseButtonUp(0))
        {

            // convert float location to int location
            int start_x = Mathf.FloorToInt(startDragPosition.x);
            int end_x = Mathf.FloorToInt(curFramePosition.x);
          
            //invert if going right to left
            if (end_x < start_x)
            {
                int tmp = end_x;
                end_x = start_x;
                start_x = tmp;
            }

            // convert float location to int location
            int start_y = Mathf.FloorToInt(startDragPosition.y);
            int end_y = Mathf.FloorToInt(curFramePosition.y);

            //invert if going right to left
            if (end_y < start_y)
            {
                int tmp = end_y;
                end_y = start_y;
                start_y = tmp;
            }

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldManager.Instance.GetTileAT(x, y);
                    if (t != null)
                    {
                        //spawn a yellow game object at the coords passed and parent it to the selection manager
                        Vector3 tmpLocation = new Vector3(x, y, 0);
                        GameObject go = (GameObject)SimplePool.Spawn(SelectedHighLight, tmpLocation, Quaternion.identity);
                        go.transform.SetParent(this.transform);
                       
                        //add the tile under yellow game object to a list
                        SelectedTileList.Add(t);

                        //get the spriterenderer and assign the sorting layer to highlights
                        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                        sr.sortingLayerName = "HighLights";

                        //add the yellow game object to a list 
                        YellowGOList.Add(go);

                    }
                }
            }

            // loop through all the yellow preview game objects and despawn them all.
            // then remove those gameobjects from their list
            for (int p = 0; p < PreviewHighLightTiles.Count; p++)
            {
                SimplePool.Despawn(PreviewHighLightTiles[p]);
            }
            PreviewHighLightTiles.Clear();



        }



    }

    // Destroy any prefabs of SelectedHighLight found in the list of Highlights
    // clear the list of Highlight tiles.
    public void DestroyHighlight()
    {
        foreach (GameObject go in YellowGOList)
            SimplePool.Despawn(go);

        SelectedTileList.Clear();
        YellowGOList.Clear();
    }


    // this converts the world cords from floats to int so they match up with tiles names
    public Tile SelectTile(Vector3 cord)
    {
        int x = Mathf.FloorToInt(cord.x);
        int y = Mathf.FloorToInt(cord.y);
        Tile t = WorldManager.Instance.GetTileAT(x, y);
        return t;

    }





 



}
