using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  enum TimeOfDay { Night, PreDawn, Dawn,PostDawn, Day,PreDusk, Dusk, PostDusk };

public class DayNightCycle : MonoBehaviour {

    public static DayNightCycle Instance;


    public float Daytime = 500f;
    public float NightTime = 100f;

    
    public float timer = 0f;


    public bool IsNightTime = false;
   // bool dusk = false;
    //bool dawn = false;



    //set colors for the alpha for the color mask on daynight cycle
   

    public float preDawnAlpha = .6f;

    public float dawmAlpha = .4f;

    public float postDawnAlpha = .2f;

    private float dayAlpha = 0f;

    public float preDuskAlpha = .2f;

    public float duskAlpha = .4f;

    public float postDuskAlpha = .6f;

    public float nightAlpha = .8f;



    [Header("Debugging only")]
    public float WhatMyAlphaIsNow;

    Color preDawnColor;
    Color dawnColor;
    Color postDawnColor;
    Color dayColor;
    Color preDuskColor;
    Color duskColor;
    Color postDuskColor;
    Color nightColor;
   
    

   






    public SpriteRenderer DarknessSprite;

    public TimeOfDay timeOfDay;
    public void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
      


        DarknessSprite = GetComponent<SpriteRenderer>();

        preDawnColor = DarknessSprite.color;
        preDawnColor.a = preDawnAlpha;

        dawnColor = DarknessSprite.color;
        dawnColor.a = dawmAlpha;

        postDawnColor = DarknessSprite.color;
        postDawnColor.a = postDawnAlpha;

        dayColor = DarknessSprite.color;
        dayColor.a = dayAlpha;

        preDuskColor = DarknessSprite.color;
        preDuskColor.a = preDuskAlpha;

        duskColor = DarknessSprite.color;
        duskColor.a = duskAlpha;

        postDuskColor = DarknessSprite.color;
        postDuskColor.a = postDuskAlpha;

        nightColor = DarknessSprite.color;
        nightColor.a = nightAlpha;

       

        

       
    }
	
	// Update is called once per frame
	void Update ()
    {

       

        WhatMyAlphaIsNow = DarknessSprite.color.a;
       


      //  if (IsNightTime)
       //     DarknessSprite.color = nightColor;
        
      //  if(!IsNightTime)
       //     DarknessSprite.color = dayColor;


        timer += Time.deltaTime;

        //PreDuskSetting
        if (timer > (Daytime * .66f) && !IsNightTime)
        {
            timeOfDay = TimeOfDay.PreDusk;
        }
        //Dusk Setting
        if (timer >(Daytime *.75f) && !IsNightTime)
        {
            timeOfDay = TimeOfDay.Dusk;
        }
        //PostDusk Setting
        if (timer > (Daytime * .88f) && !IsNightTime)
        {
            timeOfDay = TimeOfDay.PostDusk;
        }
        //Night Setting
        if (timer > Daytime && !IsNightTime)
        {
            IsNightTime = true;
            timer = 0f;
            timeOfDay = TimeOfDay.Night;
        }
        //PreDawn Setting
        if (timer > (NightTime * .66f) && IsNightTime)
        {
            timeOfDay = TimeOfDay.PreDawn;
        }
        //Dawn Setting
        if (timer > (NightTime * .75f) && IsNightTime)
        {
            timeOfDay = TimeOfDay.Dawn;
        }
        //PostDawm Setting
        if (timer > (NightTime * .88f) && IsNightTime)
        {
            timeOfDay = TimeOfDay.PostDawn;
        }



        if (timer > NightTime && IsNightTime)
        {
            IsNightTime = false;
            timer = 0f;
            timeOfDay = TimeOfDay.Day;

        }




        ChangeAlpha();


    }


    void ChangeAlpha()
    {
        switch (timeOfDay)
        {

            case TimeOfDay.PreDusk:
                DarknessSprite.color = preDuskColor;
                break;

            case TimeOfDay.Dusk:
                DarknessSprite.color = duskColor;
                break;

            case TimeOfDay.PostDusk:
                DarknessSprite.color = postDuskColor;
                break;

            case TimeOfDay.Night:
                DarknessSprite.color = nightColor;
                break;

            case TimeOfDay.PreDawn:
                DarknessSprite.color = preDawnColor;
                break;

            case TimeOfDay.Dawn:
                DarknessSprite.color = dawnColor;
                break;

            case TimeOfDay.PostDawn:
                DarknessSprite.color = postDawnColor;
                break;

            case TimeOfDay.Day:
                DarknessSprite.color = dayColor;
                break;
                            
           

           
        }
           

    }








}
