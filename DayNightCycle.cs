using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  enum TimeOfDay { Night, Dawn, Day, Dusk };

public class DayNightCycle : MonoBehaviour {

    public static DayNightCycle Instance;


    public float Daytime = 500f;
    public float NightTime = 100f;

    
    public float timer = 0f;


    public bool IsNightTime = false;
    bool dusk = false;
    bool dawn = false;



    //set colors for the alpha for the color mask on daynight cycle
    [Range(0, .8f)]
    public float nightAlpha = .8f;

    public float duskAlpha = .4f;

    float dayAlpha = 0f;
    Color dayColor;
    Color nightColor;
    Color DuskColor;

   






    public SpriteRenderer DarknessSprite;

    public TimeOfDay timeOfDay;
    public void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
      


        DarknessSprite = GetComponent<SpriteRenderer>();
        dayColor = DarknessSprite.color;
        dayColor.a = dayAlpha;
        nightColor = DarknessSprite.color;
        nightColor.a = nightAlpha;
        DuskColor = DarknessSprite.color;
        DuskColor.a = duskAlpha;

        
	}
	
	// Update is called once per frame
	void Update () {

      //  if (IsNightTime)
       //     DarknessSprite.color = nightColor;
        
      //  if(!IsNightTime)
       //     DarknessSprite.color = dayColor;


        timer += Time.deltaTime;

        
        if(timer >(Daytime *.75f) && !IsNightTime)
        {
            timeOfDay = TimeOfDay.Dusk;
        }

        if(timer > Daytime && !IsNightTime)
        {
            IsNightTime = true;
            timer = 0f;
            timeOfDay = TimeOfDay.Night;
        }

        if (timer > NightTime && IsNightTime)
        {
            IsNightTime = false;
            timer = 0f;
            timeOfDay = TimeOfDay.Day;

        }

        if(timer > (NightTime * .75f)&& IsNightTime)
        {
            timeOfDay = TimeOfDay.Dawn;
        }




        ChangeAlpha();


    }


    void ChangeAlpha()
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Dusk:
                DarknessSprite.color = Color.Lerp(nightColor,DuskColor,1f);
                
                    

                    break;
                
                    
                   
            case TimeOfDay.Night:
                DarknessSprite.color = nightColor;
                break;

            case TimeOfDay.Dawn:
                DarknessSprite.color = DuskColor;
                break;

            case TimeOfDay.Day:
                DarknessSprite.color = dayColor;
                break;
        }
           

    }








}
