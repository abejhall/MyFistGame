using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour {

    public GameObject CharacterToSpawn;
    public bool CharacterHasSpawned = false;
    public int numberOfCharacters = 2;
    int countCharacters = 0;
    public GameObject CharacterMask;
    public GameObject Torch;

    public float Xoffset = 1.6f;
    public float Yoffset = 3.5f;

    Vector3 center;

	// Use this for initialization
	void Start () {
        center = new Vector3(WorldManager.Instance.WorldHeight/2, WorldManager.Instance.WorldWidth/2, 0);
	}
	
	// Update is called once per frame
	void Update () {

        if (countCharacters < numberOfCharacters)
            CharacterHasSpawned = false;
        
        if (!CharacterHasSpawned)
        {
            SpawnCharacter();
        }
        
	}

    void SpawnCharacter()
    {
        
        GameObject go = GameObject.Instantiate(CharacterToSpawn, center, Quaternion.identity);
        WorldManager.Instance.PlayerCharacters.Add(go);
        GameObject mask = GameObject.Instantiate(CharacterMask, go.transform.position, Quaternion.identity);
        
        mask.GetComponent<followcharacter>().character = go;

        CharacterHasSpawned = true;
        countCharacters += 1;
       // Vector3 goPlus = new Vector3(go.transform.position.x + Xoffset, go.transform.position.y + Yoffset, 0);
        GameObject torch = GameObject.Instantiate(Torch, go.transform.position, Quaternion.identity);

        torch.GetComponent<followcharacter>().character = go;
       // torch.transform.parent = go.transform;
        torch.transform.localPosition = new Vector3(1.8f,4.6f,0);
        

    }

}
