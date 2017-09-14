using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooseMaterial : MonoBehaviour {

    public Sprite mySprite;
    public int MyCounterTotal;
    public string myType;

    public int MaxStackSize;
    public Text CounterText;

    public string baseType;

    public Tile MyTile;

    public bool SomeOneIsComingForMe = false;

    public bool ShouldIShutDown = false;
	
	// Update is called once per frame
	void Update () {

        
        if (CounterText != null && !ShouldIShutDown)
        {
            CounterText.text = MyCounterTotal.ToString();

            if (MyCounterTotal <= 0)//(CounterText.text == "0")
            {
                ShouldIShutDown = true;
                if(WorldManager.Instance.LooseMaterialsMap.ContainsKey(MyTile))
                WorldManager.Instance.LooseMaterialsMap.Remove(MyTile);

                Destroy(gameObject);
            }
        }
        if(CounterText == null && !ShouldIShutDown)
        {
            CounterText = GetComponentInChildren<Text>();
        }
    }
}
