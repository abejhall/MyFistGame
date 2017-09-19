using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour {

    public static TriggerDoor instance;

    public float DoorWait = 4f;
    float waitingTime;
    //bool waitingForDoor = false;
    bool startedTimer = false;
    GameObject DoorGoPointer;
    public float q1 = 1;
   // Character thischar;


    // Use this for initialization
    void Start () {
        instance = this;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Waiting(Character c, GameObject go)
    {
        DoorGoPointer = go;
        //thischar = c;
        float doorslider;


        if (!startedTimer)
        {
            waitingTime = Time.time + DoorWait;
            q1 = DoorWait / 4;
            startedTimer = true;
        }
        // Debug.Log("waitingTime; "+waitingTime);
        // Debug.Log("Time"+Time.time);
        if (waitingTime + (q1 * 3) <= Time.time)
        {

           // waitingForDoor = false;
            startedTimer = false;
        }


        doorslider = waitingTime - (q1 * 3);
        if (doorslider <= Time.time)
        {
            Debug.Log("start q1");
            SpriteRenderer sr = DoorGoPointer.GetComponent<SpriteRenderer>();
            sr.sprite = BuildManager.Instance.doors[1];
        }

        doorslider = waitingTime - (q1 * 2);
        if (doorslider <= Time.time)
        {
            Debug.Log("start q2");
            SpriteRenderer sr = DoorGoPointer.GetComponent<SpriteRenderer>();
            sr.sprite = BuildManager.Instance.doors[2];
        }

        doorslider = waitingTime - (q1);
        if (doorslider <= Time.time)
        {
            Debug.Log("start q3");
            SpriteRenderer sr = DoorGoPointer.GetComponent<SpriteRenderer>();
            sr.sprite = BuildManager.Instance.doors[1];
            //thischar.Islerping = true;
        }
        doorslider = waitingTime;
        if (doorslider <= Time.time)
        {
            Debug.Log("start q4");
            SpriteRenderer sr = DoorGoPointer.GetComponent<SpriteRenderer>();
            sr.sprite = BuildManager.Instance.doors[0];

        }
        if ((doorslider + q1 * 3) <= Time.time)
        {
            Debug.Log("start q4");
            SpriteRenderer sr = DoorGoPointer.GetComponent<SpriteRenderer>();
            sr.sprite = BuildManager.Instance.doors[0];

        }
    }






}
