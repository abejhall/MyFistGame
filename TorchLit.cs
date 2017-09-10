using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLit : MonoBehaviour {

    
    SpriteRenderer sr;
    Color torchlit;
    Color torchOut;
    Color tHalfL;
    [Range(0,.29f)]
    public float tl = .29f;
    public float tHalf = .14f;
    float to = 0f;
    bool isNightTime;

    

	// Use this for initialization
	void Start () {
        isNightTime = DayNightCycle.Instance.IsNightTime;
        sr = GetComponent<SpriteRenderer>();

        torchlit = sr.color;
        torchOut = sr.color;
        tHalfL = sr.color;
        torchlit.a = tl;
        torchOut.a = to;
        tHalfL.a = tHalf;
	}

    // Update is called once per frame
    void Update() {

        isNightTime = DayNightCycle.Instance.IsNightTime;

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Night)
        {
            sr.color = torchlit;
        }

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Day)
        {
            sr.color = torchOut;
        }

        if (DayNightCycle.Instance.timeOfDay == TimeOfDay.Dusk || DayNightCycle.Instance.timeOfDay == TimeOfDay.Dawn)
        {
            sr.color = tHalfL;
        }

    }
}
