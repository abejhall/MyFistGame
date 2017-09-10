using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followcharacter : MonoBehaviour {

    public GameObject character;
    public float xoffset = .01f;
    public float yoffset = .01f;
	// Use this for initialization
	void Start () {
        //character = GameObject.FindGameObjectWithTag("Character");
	}
	
	// Update is called once per frame
	void Update () {
        if(character != null)
        this.transform.position = new Vector3(character.transform.position.x - xoffset,character.transform.position.y - yoffset,0);

        if (character == null)
            Start();

    }
}
