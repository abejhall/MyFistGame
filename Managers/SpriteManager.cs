using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {


    public static SpriteManager Instance;

    public Sprite[] SPRITES;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	public Sprite GS(string spriteName)
    {
        for (int i = 0; i < SPRITES.Length; i++)
        {
            if (SPRITES[i].name == spriteName)
            {
                return SPRITES[i];
            }
        }

        Debug.LogError("SpriteManager did not find a sprite with this name on it: " + spriteName);
        return null;
    }
}
