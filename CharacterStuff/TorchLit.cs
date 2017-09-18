using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLit : MonoBehaviour {

    
    SpriteRenderer sr;
    Color tourchLitColor;
    Color torchOutColor;
    Color tHalfColor;
    Color tQuartColor;
    [Range(0,.29f)]
    public float tl = .29f;
    public float tHalf = .14f;
    public float tQuart = .07f;
    float to = 0f;
    //bool isNightTime; warning says never used

    

	// Use this for initialization
	void Start () {
       // isNightTime = DayNightCycle.Instance.IsNightTime; // warning says it is never used
        sr = GetComponent<SpriteRenderer>();

        tourchLitColor = sr.color;
        torchOutColor = sr.color;
        tHalfColor = sr.color;
        tQuartColor = sr.color;
        tourchLitColor.a = tl;
        torchOutColor.a = to;
        tHalfColor.a = tHalf;
        tQuartColor.a = tQuart;
        
	}

    // Update is called once per frame
    void Update() {

        //isNightTime = DayNightCycle.Instance.IsNightTime; //warning says it is never used

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Night)
        {
            sr.color = tourchLitColor;
        }

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Day)
        {
            sr.color = torchOutColor;
        }

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Dusk || DayNightCycle.Instance.timeOfDay == TimeOfDay.Dawn|| DayNightCycle.Instance.timeOfDay == TimeOfDay.PostDusk || DayNightCycle.Instance.timeOfDay == TimeOfDay.PreDawn )
        {
            sr.color = tHalfColor;
        }

        if(DayNightCycle.Instance.timeOfDay == TimeOfDay.PostDawn || DayNightCycle.Instance.timeOfDay == TimeOfDay.PreDusk )
        {
            sr.color = tQuartColor;
        }
    }
}
