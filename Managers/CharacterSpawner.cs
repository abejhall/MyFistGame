using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour {

    public GameObject CharacterToSpawn;
    public bool CharacterHasSpawned = false;
    public int numberOfCharacters = 2;

    Vector3 center;

	// Use this for initialization
	void Start () {
        center = new Vector3(50, 50, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (!CharacterHasSpawned)
        {
           
            GameObject go = GameObject.Instantiate(CharacterToSpawn, center, Quaternion.identity);
            
            CharacterHasSpawned = true;
        }
	}
}
