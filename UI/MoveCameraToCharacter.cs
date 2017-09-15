using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToCharacter : MonoBehaviour {

    PanelComponents thisPanelsComponets;
    Camera MainCamera;


    Vector3 tmpVec3;
    Vector3 Dest;
   

    // Use this for initialization
    void Start () {
        MainCamera = Camera.main;
        thisPanelsComponets = GetComponentInParent<PanelComponents>();
	}
	
	// Update is called once per frame
	void Update () {
       

    }


   public void MoveCamToCharacter()
    {
        tmpVec3 = thisPanelsComponets.MyCharacter.transform.position;
        Dest = new Vector3(tmpVec3.x, tmpVec3.y, MainCamera.transform.position.z);
        MainCamera.transform.position = Dest;
      
    }
}
