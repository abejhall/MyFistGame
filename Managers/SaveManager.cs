using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour {

    public TestPlayerData playerData;
    string FilePath;

	// Use this for initialization
	void Start () {
        FilePath = Path.Combine(Application.dataPath, "save.txt");
        Debug.Log("saving");
        Save();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Save()
    {
        string jsonString = JsonUtility.ToJson(playerData);
        File.WriteAllText(FilePath, jsonString);
    }

    private void Load()
    {
        string jsonString = File.ReadAllText(FilePath);
        JsonUtility.FromJsonOverwrite(jsonString, playerData);
    }

}
