using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {


    public string Name;
    [Range(0,100)]
    public float Health = 100;
    [Range(0, 100)]
    public float Food = 100;




	// Use this for initialization
	void Start () {
        if (Name == "")
        {
            Name = CharacterNames.Instant.GetMaleFistName();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
