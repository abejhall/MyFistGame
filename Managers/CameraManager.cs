using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public WorldManager WM;
    public float CameraMovementSpeed = .5f;
    public float ScrollSpeed = 3f;

    Vector3 LastFramePosition;

	// Use this for initialization
	void Start () {
     
         Camera.main.transform.position = new Vector3(WM.WorldHeight/2, WM.WorldWidth/2, Camera.main.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        //get axis from WASD or joysticks and allow for moving the camera this way
        GetAxisForWSD();

        //clamp the size for zooming in and out of screen
        ClampCamera();

        //this allows the camera to move via the center or right mouse button
        MoveViaMouseButtons();
    }


    //clamp the size for zooming in and out of screen
        void ClampCamera()
         {

        if (Camera.main.orthographicSize > 15f)
            Camera.main.orthographicSize = 15f;


        if (Camera.main.orthographicSize < 3.5f)
            Camera.main.orthographicSize = 3.5f;
         }


    //get axis from WASD or joysticks and allow for moving the camera this way
    void GetAxisForWSD()
    {
        if (Input.GetAxis("Vertical") > .1f)
        {
            Camera.main.transform.Translate(Vector3.up * Time.deltaTime * CameraMovementSpeed);
        }
        if (Input.GetAxis("Vertical") < -0.1f)
        {
            Camera.main.transform.Translate(Vector3.up * Time.deltaTime * CameraMovementSpeed * -1);
        }

        if (Input.GetAxis("Horizontal") > .1f)
        {
            Camera.main.transform.Translate(Vector3.right * Time.deltaTime * CameraMovementSpeed);
        }
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            Camera.main.transform.Translate(Vector3.right * Time.deltaTime * CameraMovementSpeed * -1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            Camera.main.orthographicSize = (Camera.main.orthographicSize - (Time.deltaTime * ScrollSpeed));
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Camera.main.orthographicSize = (Camera.main.orthographicSize + (Time.deltaTime * ScrollSpeed));
        }
    }

    //this allows the camera to move via the center or right mouse button
    private void MoveViaMouseButtons()
    {
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        if(Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
          
            Vector3 diff = LastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);

        }
        LastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LastFramePosition.z = 0;
    }

}
