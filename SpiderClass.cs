using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderClass : MonoBehaviour {

    public Tile MyTile;
    public Vector3 MyTransform;
    public float MoveSpeed;
    public Tile NextTile;
    public Vector3 NextTransform;

    public bool ReachedMyNextTile = true;
    public bool ChangeDirection = false;
    float _step;
    float StartTimer;
    public float dist;
    Animator anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
      //  MyTile = WorldManager.Instance.GetTileAT((int)transform.position.x, (int)transform.position.y);

	}
	
	// Update is called once per frame
	void Update () {
        MyTile = WorldManager.Instance.GetTileAT((int)transform.position.x, (int)transform.position.y);

        MyTransform = new Vector3(MyTile.x, MyTile.y, 0);

        if ( NextTile != null) 
        NextTransform = new Vector3(NextTile.x,NextTile.y,0);

        if (NextTile != null)
            dist = Vector3.Distance(transform.position, GetNexPosition());

        //If I have reached my destination get a new one and switch bool to start moving
        if (ReachedMyNextTile)
        {
            NextTile = GetNextTile(MyTile);

            ChangeDirection = true;

            Invoke("WaitToMove", 3f);
        }

        //check and see if i have reached my destination
       
        if (MyTile == NextTile)
        {
            ReachedMyNextTile = true;
          //  anim.SetBool("IsWalking", false);
        }
            

        //set a timer for lerping
        _step = (Time.time - StartTimer) * MoveSpeed;

        

        //start moving if new des is not current des
        if (!ReachedMyNextTile)
        {
            transform.position = Vector3.Lerp(this.transform.position, GetNexPosition(), _step);
            anim.SetBool("IsWalking", true);
        }
        
    }



    void IfClosePlayIdle()
    {
        if(dist >= 1)
        {
            anim.SetBool("IsWalking", false);
        }
        //else 
          //  anim.SetBool("IsWalking", true);
    }


    void WaitToMove()
    {
        if (NextTile.MovementSpeedAdjustment != 0 && ChangeDirection)
        {
            CheckDirection();
            ChangeDirection = false;
        }

        ReachedMyNextTile = false;
        
        //anim.SetBool("IsWalking", true);
    }



    Vector3 GetMyPosition()
    {
        return new Vector3(MyTile.x,MyTile.y,0);
                 
    }

    Vector3 GetNexPosition()
    {
        return new Vector3(NextTile.x, NextTile.y, 0);
    }

   Tile GetNextTile(Tile t)
    {
        int ran = Random.Range(0, 3);
        switch (ran)
        {
            case 0:
                t = WorldManager.Instance.GetTileAT(MyTile.x+1, MyTile.y);
                if(t.MovementSpeedAdjustment == 0)
                        { return MyTile;}
                StartTimer = Time.time;
                
                return t;
               
            case 1:
                t = WorldManager.Instance.GetTileAT(MyTile.x , MyTile.y-1);
                if (t.MovementSpeedAdjustment == 0)
                        { return MyTile; }
                StartTimer = Time.time;
                
                return t;
               
            case 2:
                t = WorldManager.Instance.GetTileAT(MyTile.x - 1, MyTile.y);
                if (t.MovementSpeedAdjustment == 0)
                        { return MyTile; }
                StartTimer = Time.time;
               
                return t;
               
            case 3:
                t = WorldManager.Instance.GetTileAT(MyTile.x, MyTile.y+1);
                if (t.MovementSpeedAdjustment == 0)
                        { return MyTile; }
                StartTimer = Time.time;
                 return t;
               
            default:
             return  MyTile;
               

        }
       
    }


    void CheckDirection()
    {
        if (NextTile.x == MyTile.x && NextTile.y == MyTile.y)
            return;

       else if (NextTile.x > MyTile.x)
        {
            RotateLeft();
            return;
        }
            

      else  if (NextTile.x < MyTile.x)
        {
            RotateRight();
            return;
        }
          

      else  if (NextTile.y > MyTile.y)
        {
            RotateUp();
            return;
        }
            

      else if (NextTile.y < MyTile.y)
        {
            RotateDown();
            return;
        }
            
    }


    void RotateRight()
    {

        //  transform.Rotate(0,0,90);
        transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    void RotateLeft()
    {
      
            //transform.Rotate(0, 0, -90);
        transform.localEulerAngles = new Vector3(0, 0, -90);
    }

    void RotateUp()
    {

        // transform.Rotate(0, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    void RotateDown()
    {

        //transform.Rotate(180, 0, 0);
        transform.localEulerAngles = new Vector3(180, 0, 0);
    }

}
