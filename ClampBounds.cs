using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampBounds : MonoBehaviour {


   
    public float xMin = 0;
    public float xMax = 199;
    public float yMin = 0;
    public float yMax = 0;


    Transform t;

    private void Start()
    {
        t = transform;

    }

    private void LateUpdate()
    {
        Vector3 MyTarget = transform.position;

        float x = Mathf.Clamp(MyTarget.x, xMin, xMax);
        float y = Mathf.Clamp(MyTarget.y, yMin, yMax);
        t.position = new Vector3(x, y, t.position.z);
    }

}
