using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpriteSwap : MonoBehaviour {


    public SpriteRenderer idle;
    public SpriteRenderer walking;
    public SpriteRenderer chopping;
    CharacterStateMachine state;


	// Use this for initialization
	void Start () {
        state = GetComponent<CharacterStateMachine>();
        walking.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

       if(state.CurrentState == CharacterStateMachine.StateMachine.MoveToJob || state.CurrentState == CharacterStateMachine.StateMachine.MoveToMaterial )
        {
            idle.enabled = false;
            walking.enabled = true;
            chopping.enabled = false;
        }
       else if(state.CurrentState == CharacterStateMachine.StateMachine.Working)
        {
            idle.enabled = false;
            walking.enabled = false;
            chopping.enabled = true;
        }
        else
        {
            idle.enabled = true;
            walking.enabled = false;
            chopping.enabled = false;

        }
		
	}
}
