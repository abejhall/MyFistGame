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

    
	
	// Update is called once per frame
	void Update () {
        if (CounterText != null)
        {
            CounterText.text = MyCounterTotal.ToString();

            if (MyCounterTotal <= 0)//(CounterText.text == "0")
            {
                Destroy(gameObject);
            }
        }
        if(CounterText == null)
        {
            CounterText = GetComponentInChildren<Text>();
        }
    }
}
